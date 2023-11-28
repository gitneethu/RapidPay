using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API_CreditCard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace API_CreditCard.Controllers
{
   

   
       
        
        [ApiController]
        [Route("api/cards")]
        public class CardsController : ControllerBase
        {
           
            private readonly GeneratePaymentFee _randomDecimalGenerator;
            bool validAPI = false;

        public CardsController(GeneratePaymentFee randomDecimalGenerator)
            {
               
                _randomDecimalGenerator = randomDecimalGenerator;
            }

            [HttpPost]
           // [Authorize("CreateCardPolicy")] 
            public IActionResult CreateCard()
            {

            string cardNumber = "";
            string expiryDate = "";
            int cvv;
           
            bool existingcard = false;

            // Ensure the API Key  is valid
            // Retrieve the API key from the request headers
            string ?apiKey = HttpContext.Request.Headers["Authorization"];
        
                if (string.IsNullOrEmpty (apiKey ))
                {
                    return Unauthorized();
                }

             validAPI = InMemoryDB.CheckAPI(apiKey);
            if (validAPI )
            {
                //Credit card creation .Generate a random 15-digit card number.
                cardNumber = generateRandomCardNumber();

                // Set expiry date for the card
                expiryDate = generateExpiryDate();

                // Set the cvv number for the credit card
                cvv = generateCVV();
                var newCard = new CreditCard
                {
                    CardNumber = cardNumber,
                    Balance = 0,
                    ExpiryDate = expiryDate,
                    Cvv = cvv

                };

                // Before storing the card number into database, check if the same number already exists in database.
                 existingcard = InMemoryDB.CheckCreditCard(cardNumber);
                if (existingcard)
                {
                    //Send a response back
                }
                else
                {
                    InMemoryDB.AddUpdateCreditCard(newCard);
                  
                }
               
            }
            else
            {

                return Unauthorized ();
            }

            return Ok(new { Message = "Card created successfully.", CardNumber = cardNumber });
        }

          

            private string generateRandomCardNumber()
            {
                Random random = new Random();

                long lowerBound = 100_000_000_000_000;
                long upperBound = 999_999_999_999_999;
                long randomLong = (long)(random.NextDouble () * (upperBound - lowerBound) + lowerBound);
                return randomLong.ToString();
            }

            private string generateExpiryDate()
            {

                // Add 3 years to the current date
                DateTime dateInThreeYears = DateTime.Today.AddYears(3);
                string dateInThreeYearsString = dateInThreeYears.ToString("MM-dd-yyyy");
                return dateInThreeYearsString;
            }

            private int generateCVV()
            {

            // Generate a random 3-digit number
            Random random = new Random();
            int randomNumber = random.Next(100, 1000);
                return randomNumber;
            }

            [HttpPost("pay")]
            public IActionResult Pay([FromBody] Request paymentRequest)
            {

            string inCreditcardNo = "";
            int inCvv;
            string inExpiry = "";
            decimal paymentAmount;

            decimal randomDecimal;
            decimal lastFeeAmount;
            decimal newFeeAmount;

            string? apiKey = HttpContext.Request.Headers["Authorization"];

            if (string.IsNullOrEmpty(apiKey))
            {
                return Unauthorized();
            }

             validAPI = InMemoryDB.CheckAPI(apiKey);

            //Receive the creditcardno, cvv,expiry,lastfee from the receiver
            inCreditcardNo = paymentRequest.CardNumber;
            inCvv = paymentRequest.Cvv;
            inExpiry = paymentRequest.ExpiryDate;
            paymentAmount = paymentRequest.Amount;

            // Retrieve the credit card details from Dictionary (In Memory).
           
            var creditCard = InMemoryDB.GetCreditCardDetails(inCreditcardNo);
                if (creditCard == null)
                {
                    return NotFound("Credit card not found for the user.");
                }

                // Verify the cvv and expirydate associated with the card

                if((creditCard.Cvv==inCvv  )&&(creditCard.ExpiryDate ==inExpiry ))
                 {
                     //Verified. Pay the card.

                     if (creditCard.Balance < paymentAmount)
                     {
                          return BadRequest("Insufficient funds.");
                     }

                   
                    // Generate a random decimal for additional processing.
                     randomDecimal = _randomDecimalGenerator.GenerateRandomDecimal();

                   // The new fee price is the last fee amount multiplied by the recent random decimal.
                   // If Last Fee Amount is null , it will be taken as 0
                     lastFeeAmount = creditCard.LastFeeAmount ?? 0;
                     newFeeAmount = lastFeeAmount * randomDecimal;

                  // Update the credit card balance and fee amount, then save to the database
                    creditCard.Balance = creditCard.Balance -paymentAmount + newFeeAmount;
                    creditCard.LastFeeAmount = newFeeAmount;
                 InMemoryDB.AddUpdateCreditCard(creditCard);

            }
                else
                 {
                    //CVV OR ExpiryDate Mismatch.
                    return Unauthorized();
                 }
             
                 return Ok(new { Message = "Payment successful.", NewBalance = creditCard.Balance, NewFeeAmount = newFeeAmount });
               
            }

            [HttpGet("balance")]
            public IActionResult GetCardBalance([FromBody] Request  creditBalance)
            {

            string inCreditcardNo = "";
            string? apiKey = HttpContext.Request.Headers["Authorization"];

            if (string.IsNullOrEmpty(apiKey))
            {
                return Unauthorized();
            }

            validAPI = InMemoryDB.CheckAPI(apiKey);

            inCreditcardNo = creditBalance.CardNumber;

            // Retrieve the credit card details from Dictionary (In Memory).

            var creditCard = InMemoryDB.GetCreditCardDetails(inCreditcardNo);
            if (creditCard == null)
            {
                return NotFound("Credit card not found for the user.");
            }
           
                var balance = creditCard.Balance;

                if (balance == default(decimal))
                {
                    return NotFound("Credit card not found or has a zero balance for the user.");
                }

                 return Ok(new { Balance = balance }); 
            }
          
        }

    public class Request
    {
        public decimal Amount { get; set; }
        public string CardNumber { get; set; }
        public int Cvv { get; set; }
        public string ExpiryDate { get; set; }
     
    }

}








