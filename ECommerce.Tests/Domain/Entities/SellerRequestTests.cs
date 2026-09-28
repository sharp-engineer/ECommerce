using ECommerce.Domain.Entities;
using ECommerce.Domain.Enums;
using ECommerce.Domain.Exceptions;

namespace ECommerce.Tests.Domain.Entities;

public sealed class SellerRequestTests
{
    [Test]
    public void Create_should_create_pending_request()
    {
        var userId = Guid.NewGuid();

        var request = SellerRequest.Create(
            userId,
            "I want to become a seller.");

        Assert.Multiple(() =>
        {
            Assert.That(request.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(request.UserId, Is.EqualTo(userId));
            Assert.That(
                request.Status,
                Is.EqualTo(SellerRequestStatus.Pending));
            Assert.That(
                request.Reason,
                Is.EqualTo("I want to become a seller."));
            Assert.That(
                request.ReviewedByUserId,
                Is.Null);
            Assert.That(
                request.ReviewedAt,
                Is.Null);
        });
    }

    [Test]
    public void Create_should_reject_empty_user_id()
    {
        Assert.Throws<DomainException>(() =>
            SellerRequest.Create(
                Guid.Empty,
                "I want to become a seller."));
    }

    [Test]
    public void Approve_should_approve_pending_request()
    {
        var userId = Guid.NewGuid();
        var adminUserId = Guid.NewGuid();

        var request = SellerRequest.Create(
            userId,
            "I want to become a seller.");

        request.Approve(adminUserId);

        Assert.Multiple(() =>
        {
            Assert.That(
                request.Status,
                Is.EqualTo(SellerRequestStatus.Approved));

            Assert.That(
                request.ReviewedByUserId,
                Is.EqualTo(adminUserId));

            Assert.That(
                request.ReviewedAt,
                Is.Not.Null);
        });
    }

    [Test]
    public void Approve_should_reject_empty_admin_user_id()
    {
        var request = SellerRequest.Create(
            Guid.NewGuid(),
            "I want to become a seller.");

        Assert.Throws<DomainException>(() =>
            request.Approve(Guid.Empty));
    }

    [Test]
    public void Approve_should_reject_non_pending_request()
    {
        var request = SellerRequest.Create(
            Guid.NewGuid(),
            "I want to become a seller.");

        request.Approve(Guid.NewGuid());

        Assert.Throws<DomainException>(() =>
            request.Approve(Guid.NewGuid()));
    }

    [Test]
    public void Reject_should_reject_pending_request()
    {
        var userId = Guid.NewGuid();
        var adminUserId = Guid.NewGuid();

        var request = SellerRequest.Create(
            userId,
            "I want to become a seller.");

        request.Reject(adminUserId);

        Assert.Multiple(() =>
        {
            Assert.That(
                request.Status,
                Is.EqualTo(SellerRequestStatus.Rejected));

            Assert.That(
                request.ReviewedByUserId,
                Is.EqualTo(adminUserId));

            Assert.That(
                request.ReviewedAt,
                Is.Not.Null);
        });
    }

    [Test]
    public void Reject_should_reject_empty_admin_user_id()
    {
        var request = SellerRequest.Create(
            Guid.NewGuid(),
            "I want to become a seller.");

        Assert.Throws<DomainException>(() =>
            request.Reject(Guid.Empty));
    }

    [Test]
    public void Reject_should_reject_non_pending_request()
    {
        var request = SellerRequest.Create(
            Guid.NewGuid(),
            "I want to become a seller.");

        request.Reject(Guid.NewGuid());

        Assert.Throws<DomainException>(() =>
            request.Reject(Guid.NewGuid()));
    }
}