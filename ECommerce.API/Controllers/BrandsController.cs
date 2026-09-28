using ECommerce.Application.Features.Catalog.Brands.CreateBrand;
using ECommerce.Application.Features.Catalog.Brands.GetBrandById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

[ApiController]
[Route("api/brands")]
public class BrandsController(ISender sender) : ControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BrandDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var brand = await sender.Send(new GetBrandByIdQuery(id), cancellationToken);
        return Ok(brand);
    }

    [HttpPost]
    public async Task<ActionResult> Create(CreateBrandCommand command, CancellationToken cancellationToken)
    {
        var brandId = await sender.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = brandId }, new { id = brandId });
    }
}