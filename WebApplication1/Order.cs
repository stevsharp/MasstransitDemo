using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using MessageContracts;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1
{
    [Route("api/order")]
    public class Order : Controller
    {
        private readonly IRequestClient<SubmitOrder, OrderAccepted> _requestClient;

        public Order(IRequestClient<SubmitOrder, OrderAccepted> requestClient)
        {
            _requestClient = requestClient;
        }

        [HttpPost]
        public async Task<IActionResult> Submit(CancellationToken cancellationToken)
        {
            try
            {
                OrderAccepted result = await _requestClient.Request(new {OrderId = DateTime.Now.Ticks}, cancellationToken);

                return Accepted(result.OrderId);
            }
            catch (RequestTimeoutException exception)
            {
                return StatusCode((int) HttpStatusCode.RequestTimeout);
            }
        }
    }
}