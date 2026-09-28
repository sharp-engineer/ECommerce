using ECommerce.Application.Features.Catalog.Products.CreateProduct;
using ECommerce.Application.Features.Catalog.Products.GetProductById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController(ISender sender) : ControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProductDetailsDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetProductByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult> Create(CreateProductCommand command, CancellationToken cancellationToken)
    {
        var productId = await sender.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = productId }, new { id = productId });
    }
}