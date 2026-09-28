using ECommerce.Application.Features.Catalog.ProductImages.CreateProductImage;
using ECommerce.Application.Features.Catalog.ProductImages.GetProductImageById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

[ApiController]
[Route("api/product-images")]
public class ProductImagesController(ISender sender) : ControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProductImageDto>> GetProductImage(Guid id, CancellationToken cancellationToken)
    {
        var image = await sender.Send(new GetProductImageByIdQuery(id), cancellationToken);
        return Ok(image);
    }

    [HttpPost]
    public async Task<ActionResult> Create(CreateProductImageCommand command, CancellationToken cancellationToken)
    {
        var imageId = await sender.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetProductImage), new { Id = imageId }, new { Id = imageId });
    }
}