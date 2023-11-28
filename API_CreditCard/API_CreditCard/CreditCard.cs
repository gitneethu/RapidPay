using System;


namespace API_CreditCard
{
	public class CreditCard
	{
        public int Id { get; set; }
        public string CardNumber { get; set; }
        public decimal Balance { get; set; }
        public int Cvv { get; set; }
        public string ExpiryDate { get; set; }
        public decimal ?LastFeeAmount { get; set; }
        
       
       //public string UserId { get; set; }
       // public ApplicationUser User { get; set; }

    }
}


