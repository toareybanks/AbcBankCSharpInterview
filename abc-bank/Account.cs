using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace abc_bank
{
    public class Account
    {

        public const int CHECKING = 0;
        public const int SAVINGS = 1;
        public const int MAXI_SAVINGS = 2;

        private readonly int accountType;
        public double yearlyRate;
        public List<Transaction> transactions;


        public Account(int accountType) 
        {
            this.accountType = accountType;
            this.transactions = new List<Transaction>();
        }

        public void Deposit(double amount) 
        {
            BankTransactionAction(amount);
        }

        public void Withdraw(double amount) 
        {
            BankTransactionAction(amount, true);
        }

        public void BankTransactionAction(double amount, bool withdraw = false)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("amount must be greater than zero");
            }
            else
            {
                transactions.Add(new Transaction(withdraw==true?-amount:amount));
            }
        }

        public double InterestEarned() 
        {
            double amount = sumTransactions();
            double ntotal =0;
            switch(accountType){
                case SAVINGS:
                    if (amount <= 1000)
                    { ntotal = CalcInterestEarned(amount);  }
                        
                    if (amount > 1000) { ntotal = CalcInterestEarned((amount - amount) + 1000) + CalcInterestEarned(amount - 1000, 1); }
                      break;  
                case MAXI_SAVINGS:
                    if (amount <= 1000)
                        ntotal = CalcInterestEarned(amount);
                    if (amount >= 1000 && amount <= 2000)
                        ntotal = CalcInterestEarned((amount - amount) + 1000) + CalcInterestEarned(amount - 1000, 1);
                    if(amount >=3000)
                        ntotal = CalcInterestEarned((amount - amount) + 1000) + CalcInterestEarned((amount - amount) + 2000, 1) + CalcInterestEarned(amount - 2000, 2);
                    break;
                default:
                    ntotal = CalcInterestEarned(amount);
                    break;
            }
            return ntotal;
        }

        public double CalcInterestEarned(double amount, int totalth = 0)
        {
            double namount=0;
            if(totalth == 0) namount += (accountType != MAXI_SAVINGS && amount == 1000? (amount * 0.01): (amount  *0.002));
            if(accountType == SAVINGS  && amount > 1000 && totalth > 0) namount += (amount * 0.002);
            if (accountType == MAXI_SAVINGS && amount > 1000 && totalth == 1) namount += (amount * 0.005);
            if (accountType == MAXI_SAVINGS && amount > 1000 && totalth > 1) namount += (amount * 0.01);
            return amount + namount;
        }

        public double CalcYearlyInterestEarned(int year)
        {
            double amount = 0;
            List<Transaction> lst = new List<Transaction>();
            lst = GetTransactions(year);
            foreach (Transaction t in lst)
                amount += t.amount;
            return CalcInterestEarned(amount);
        }

        public double sumTransactions() {
           return CheckIfTransactionsExist(true);
        }

        private double CheckIfTransactionsExist(bool checkAll) 
        {
            double amount = 0.0;
            foreach (Transaction t in transactions)
                amount += t.amount;
            return amount;
        }


        private List<Transaction> GetWithdrawnTransactions(DateTime currentdate, int numberofdays=0)
        {
            List<Transaction> lst = new List<Transaction>();
            foreach (Transaction t in transactions)
            {
                if (t.transactionDate >= currentdate.AddDays(-numberofdays) 
                        && t.transactionDate <= currentdate && t.amount <0) lst.Add(t);
            }
            return lst;
        }

        private List<Transaction> GetTransactions(DateTime currentdate)
        {
            List<Transaction> lst = new List<Transaction>();
            foreach (Transaction t in transactions)
            {
               if( t.transactionDate == currentdate) lst.Add(t);
            }
            return lst;
        }

        private List<Transaction> GetTransactions(int year)
        {
            List<Transaction> lst = new List<Transaction>();
            foreach (Transaction t in transactions)
            {
                if (t.transactionDate.Year == year) lst.Add(t);
            }
            return lst;
        }

        public int GetAccountType() 
        {
            return accountType;
        }

    }
}
