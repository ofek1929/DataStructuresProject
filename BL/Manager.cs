using BL.InnerData;
using Models;
using System;
using System.Collections.Generic;
using System.Windows.Threading;

namespace BL
{
    public class Manager
    {

        public BST<DataX> DataXTree { get; private set; }

        public DoubleLinkList<box> BoxMagazin { get; private set; }

        public int MaxItemsPerBox { get; }

        public int RequirementsReOrderMinAmount { get; }

        public DispatcherTimer CheckPeriod { get; } // interval

        public TimeSpan ExpirationDate { get; }
        public DataX X { get; set; }

        public DataY Y { get; set; }

        private DataX _xOut;// why

        private DataY _yOut;//why



        public DoubleLinkList<DateTime>.Node dateNode;//
        public Manager(int maxItemsPerBox, int requirementsReOrderMinAmount, TimeSpan checkPeriod, TimeSpan expirationDate) // ctor 
        {
            DataXTree = new BST<DataX>();
            BoxMagazin = new DoubleLinkList<box>();
            MaxItemsPerBox = maxItemsPerBox;
            RequirementsReOrderMinAmount = requirementsReOrderMinAmount;
            CheckPeriod = new DispatcherTimer();
            CheckPeriod.Interval = checkPeriod;
            CheckPeriod.Tick += CheckPeriod_Tick;
            CheckPeriod.Start();
            ExpirationDate = expirationDate;
            AddBoxes(5, 7, 25);//only for check 
            AddBoxes(5, 9, 25);//
            AddBoxes(5, 10, 5);//
        }

        //when timer thick check the dates
        private void CheckPeriod_Tick(object sender, EventArgs e)
        {

            foreach (var item in BoxMagazin)
            {
                if (BoxMagazin.start == null) return;
                else if (DateTime.Now - item.Y.lastBuyDate >= ExpirationDate) // check if the date is right to delete 
                {
                    //delete the item from the  BSts and from doubleLinkList 

                    item.X.BstY.Remove(item.Y);
                    if (item.X.BstY.CheckIfRootIsNull()) DataXTree.Remove(item.X);
                    BoxMagazin.RemoveFirst(out _);
                }
                else return; // meens that if there was one ok date all the dates after will be ok --- so doesnt need to check
            }
            //while(BoxMagazin.start != null && DateTime.Now - BoxMagazin.start.data.Y.lastBuyDate >= ExpirationDate)
            // {
            //     DataX x = new DataX(BoxMagazin.start.data.X.XSize);
            //     DataY y = new DataY(BoxMagazin.start.data.Y.YSize);
            //     DataXTree.Search(x, out x);
            //     //להעביר למשתמש הודעה איזה מהקופסאות נמחקו
            //     x.BstY.Remove(y);
            //     if (x.BstY.CheckIfRootIsNull())
            //     {
            //         DataXTree.Remove(x);
            //     }
            //     BoxMagazin.RemoveFirst(out _);
            // }
        }

        public void AddBoxes(double x, double y, int amountToBuy) // add new stok or new box to the tree 
        {
            //check if x is exsist if no add new x and y 
            //if yes chek if y exsist 
            //if yes add to the quantity if no ad new y to the x bst
            X = new DataX(x);
            Y = new DataY(y);
            DataX _xOutt = new DataX(x);// why
            DataY _yOutt = new DataY(y);//why           
            //DataX _xOut = new DataX(x);// why
            //DataY _yOut = new DataY(y);//why
            if (amountToBuy <= 1) return;
            if (DataXTree.Search(X, out _xOutt)) //check if x is exsist
            {
                if (X.BstY.Search(Y, out _yOutt))//chek if y exsist- add to the quantity by law
                {
                    if (amountToBuy + _yOutt.Quantity > MaxItemsPerBox) _yOutt.Quantity = MaxItemsPerBox;
                    else _yOutt.Quantity += amountToBuy;
                }
                else//ad new y to the x bst
                {
                    Y.Quantity = amountToBuy; // need to make sure amountToBuy is up the the min amountToBuy and lesss then the max amountToBuy
                    _xOutt.BstY.Add(Y);
                    BoxMagazin.AddLast(new box(_xOutt, Y));
                }
            }
            else//add new x and y 
            {
                Y.Quantity = amountToBuy;// need to make sure amountToBuy is up the the min amountToBuy and lesss then the max amountToBuy
                X.BstY.Add(Y);
                DataXTree.Add(X);
                BoxMagazin.AddLast(new box(X, Y));
            }
        }

        public string Info(double x, double y) // in order to show the information about the box 
        {
            //serch if the box is available 
            // if yes show .to string x and y 
            // if no make a massage that there is no box in that size
            X = new DataX(x);
            Y = new DataY(y);
            string massage;
            if (DataXTree.Search(X, out _xOut) && _xOut.BstY.Search(Y, out _yOut)) massage = $"{_xOut.ToString()},{_yOut.ToString()}";
            else massage = "there is no box in that size on stock";
            return massage;
        }
        public bool Buy(double x, double y, bool isOkToSplist, int amountOfSplits, out string result, int amountToBuy = 1) // buy new box 
        {
            result = default;
            X = new DataX(x);
            Y = new DataY(y);

            if (DataXTree.SerchTheClosest(X, out _xOut))//serch if there is x size thatok to the borders
            {
                if (_xOut.BstY.SerchTheClosest(Y, out _yOut))//serch if there is y size thatok to the borders
                {
                    //need to add a remove from bst if amountToBuy<=1
                    if (_yOut.Quantity - amountToBuy >= 0)// check if quantity is ok to sale
                    {

                        _yOut.Quantity = _yOut.Quantity - amountToBuy;
                        _yOut.lastBuyDate = DateTime.Now;
                        if (_yOut.Quantity <= 1) // need to be removed
                        {
                            _xOut.BstY.Remove(_yOut);
                            if (_xOut.BstY.CheckIfRootIsNull()) DataXTree.Remove(_xOut);
                            BoxMagazin.Delete(new box(_xOut, _yOut));
                        }
                        else BoxMagazin.MoveToEnd(new box(_xOut, _yOut));

                        result = $"{_xOut.ToString()}, {_yOut.ToStringAfterSell(amountToBuy)}";
                        return true; // sale has happened
                    }
                    else//if not enoght call to split if it is ok ---- if not ok retrn flase
                    {
                        if (isOkToSplist)
                        {
                            if (SellBySplits(x, y, amountToBuy, amountOfSplits, out result)) return true;//send the x out,y out,amountToBuy,amountofsplits to split sell func
                            else
                            {
                                result = "there is no posibole opthon to your size";
                                return false;
                            }
                        }
                    }
                }
                else
                {
                    if (DataXTree.SerchTheClosestIfExist(_xOut, out _xOut)) Buy(_xOut.XSize, y, isOkToSplist, amountOfSplits, out result, amountToBuy);
                    result = "there is no posibole opthon to your size";
                    return false;
                }
            }
            else return false;//there is no x in the size or biger so there is no box like that 

            result = "there is no posibole opthon to your size";
            return false; // to delete
        }



        //fix the name of varibals
        private bool SellBySplits(double x, double y, int amountToBuy, int numOfSplits, out string result)
        {
            result = default;
            X = new DataX(x);
            Y = new DataY(y);
            List<box> allSplit = new List<box>();
            box temp;

            int quantityWasSold = 0;
            for (int i = 0; i < numOfSplits; i++)
            {
                if (i == 0)
                {
                    DataXTree.SerchTheClosest(X, out _xOut);
                    _xOut.BstY.SerchTheClosest(Y, out _yOut);
                }
                else
                {
                    if (_xOut.BstY.SerchTheClosestIfExist(_yOut, out _yOut)) { }
                    else
                    {
                        bool flage = false;
                        while (!flage) if (DataXTree.SerchTheClosestIfExist(_xOut, out _xOut)) { flage = _xOut.BstY.SerchTheClosestIfExist(_yOut, out _yOut); }
                    }
                }
                temp = new box(_xOut, _yOut);

                quantityWasSold += _yOut.Quantity;
                allSplit.Add(temp);
                if (quantityWasSold >= amountToBuy)
                {
                    quantityWasSold = 0;
                    foreach (var item in allSplit)
                    {
                        if (amountToBuy - quantityWasSold < item.Y.Quantity)
                        {
                            //y.quantity = y.quantity - (amountToBuy - quatityWasSold)
                            item.Y.Quantity -= amountToBuy - quantityWasSold;
                            result += item.ToStringSell(amountToBuy - quantityWasSold);
                            item.Y.lastBuyDate = DateTime.Now;
                            BoxMagazin.MoveToEnd(new box(item.X, item.Y));

                            return true;
                        }
                        else if (amountToBuy - quantityWasSold == item.Y.Quantity)
                        {
                            result += item.ToStringSell(item.Y.Quantity);
                            //add to quantityWasSold 
                            quantityWasSold += item.Y.Quantity;
                            //remove frome the bst 
                            item.X.BstY.Remove(item.Y);
                            BoxMagazin.Delete(new box(item.X, item.Y));
                            if (item.X.BstY.CheckIfRootIsNull()) DataXTree.Remove(item.X);
                            return true;
                        }
                        else
                        {
                            result += item.ToStringSell(item.Y.Quantity) + "\n";
                            //add to quantityWasSold 
                            quantityWasSold += item.Y.Quantity;
                            //remove from the Doblelinklist
                            BoxMagazin.Delete(new box(item.X, item.Y));
                            //remove frome the bst 
                            item.X.BstY.Remove(item.Y);
                            if (item.X.BstY.CheckIfRootIsNull()) DataXTree.Remove(item.X);

                        }
                    }
                }
            }

            return false;
        }



    }
}





