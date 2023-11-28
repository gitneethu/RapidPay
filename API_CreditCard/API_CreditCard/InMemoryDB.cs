using System;
using System.Collections;
using System.Collections.Generic;

namespace API_CreditCard
{
    public class InMemoryDB
    {
       
        private  static readonly Dictionary<string, CreditCard> DicCreditCards = new Dictionary<string, CreditCard>();
        private static ArrayList apiKeys = new ArrayList();

        InMemoryDB ()
        {
           
            apiKeys.Add("123455667899235");
            apiKeys.Add("324567789992345");
            apiKeys.Add("123455667895635");
            apiKeys.Add("324567789992323");
            apiKeys.Add("324567789342323");

        }

        public static bool CheckAPI(string api)
        {
           if (apiKeys .Contains (api))
            {
                return true;
            }
           else
            {
                return false;
            }
        }

        public static bool CheckCreditCard(string newcardNo)
        {
            if(DicCreditCards.ContainsKey(newcardNo))
            {
                return true;
            }
            else
            {
                return false;
            }

            
        }

        public static void AddUpdateCreditCard(CreditCard newcard)
        {
            DicCreditCards[newcard.CardNumber] = new CreditCard {  Balance = newcard.Balance, LastFeeAmount = newcard .LastFeeAmount,Cvv = newcard.Cvv, ExpiryDate =newcard.ExpiryDate };

           
        }

      

        public static CreditCard GetCreditCardDetails(string cardnumber)
        {
            return DicCreditCards.TryGetValue(cardnumber, out var cardData) ? cardData : null;

         

        }
       
    }
}






