using ECommerce.Application.Features.Catalog.ProductVariants.CreateProductVariant;
using ECommerce.Application.Features.Catalog.ProductVariants.GetProductVariantById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

[ApiController]
[Route("api/product-variants")]
public class ProductVariantsController(ISender sender) : ControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProductVariantDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var variant = await sender.Send(new GetProductVariantByIdQuery(id), cancellationToken);
        return Ok(variant);
    }

    [HttpPost]
    public async Task<ActionResult> Create(CreateProductVariantsCommand command, CancellationToken cancellationToken)
    {
        var variantId = await sender.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = variantId }, new { id = variantId });
    }
}