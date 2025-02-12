using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System.ComponentModel.DataAnnotations;

namespace desafioAPI.DTO
{
    public class TransactionPostRequestBody
    {

        [Required(ErrorMessage = "You must provide a value for the transaction")]
        [Range(minimum: 0.1,maximum:15000,ErrorMessage = "The value can't be negative")]
        public decimal TransactionTotal { get; set; }
        [Required(ErrorMessage = "The receiver wallet can't be null")]
        public int receiverId { get; set; }
        [Required(ErrorMessage = "The sender wallet can't be null")]
        public int senderId { get; set; }

    }
}
