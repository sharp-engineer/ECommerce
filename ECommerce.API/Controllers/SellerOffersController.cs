using ECommerce.Application.Abstractions.Identity;
using ECommerce.Application.Features.Catalog.ProductVariants.SellerOffers.ActiveSellerOffer;
using ECommerce.Application.Features.Catalog.ProductVariants.SellerOffers.CreateSellerOffer;
using ECommerce.Application.Features.Catalog.ProductVariants.SellerOffers.DeactivateSellerOffer;
using ECommerce.Application.Features.Catalog.ProductVariants.SellerOffers.GetSellerOfferById;
using ECommerce.Application.Features.Catalog.ProductVariants.SellerOffers.UpdateSellerOfferPrice;
using ECommerce.Application.Features.Catalog.ProductVariants.SellerOffers.UpdateSellerOfferStock;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

[ApiController]
[Route("api/seller-offers")]
[Authorize(Roles = ApplicationRoles.Seller)]
public class SellerOffersController(ISender sender) : ControllerBase
{
    [AllowAnonymous]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<SellerOfferDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var offer = await sender.Send(new GetSellerOfferByIdQuery(id), cancellationToken);
        if (offer is null)
            return NotFound();

        return Ok(offer);
    }

    [HttpPost]
    public async Task<ActionResult> Create(CreateSellerOfferCommand command, CancellationToken cancellationToken)
    {
        var offerId = await sender.Send(command, cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id = offerId }, new { id = offerId });
    }

    [HttpPut("price")]
    public async Task<IActionResult> UpdatePrice(UpdateSellerOfferPriceCommand command,
        CancellationToken cancellationToken)
    {
        await sender.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpPut("stock")]
    public async Task<IActionResult> UpdateStock(UpdateSellerOfferStockCommand command,
        CancellationToken cancellationToken)
    {
        await sender.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpPut("activate")]
    public async Task<IActionResult> Activate(ActivateSellerOfferCommand command,
        CancellationToken cancellationToken)
    {
        await sender.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpPut("deactivate")]
    public async Task<IActionResult> Deactivate(DeactivateSellerOfferCommand command,
        CancellationToken cancellationToken)
    {
        await sender.Send(command, cancellationToken);
        return NoContent();
    }
}