using Microsoft.AspNetCore.Mvc;

namespace CreatTransferTransactiom.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CreateTransfereController : Controller
    {
        DDLCreateTransfere data = new DDLCreateTransfere();

        [HttpPost("/CreateTransfere")]
        public ActionResult<string> Read([FromBody] CsCreatTransferRequest x)
        {
           


            return (data.CreatTr(x.transactionType,x.To_additionalRef,x.from_additionalRef,x.transactionPurpose,x.transactionAmount,
                x.currencyIso,x.transactionDate,x.valueDate,x.userID,x.password));

        }
    }
}
