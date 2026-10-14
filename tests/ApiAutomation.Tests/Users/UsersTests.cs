using System.Net;
using ApiAutomation.Tests.Base;
using NUnit.Framework;

namespace ApiAutomation.Tests.Users;

public record User(int Id, string Name, string Username, string Email);

[TestFixture, Category("Smoke")]
public class UsersTests : ApiTestBase
{
    [Test]
    public async Task GetUsers_ReturnsOkWithNonEmptyList()
    {
        var response = await Client.GetAsync<List<User>>("/users");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(response.Data, Is.Not.Null.And.Not.Empty);
    }

    [TestCase(1)]
    [TestCase(2)]
    public async Task GetUserById_ReturnsMatchingUser(int id)
    {
        var response = await Client.GetAsync<User>($"/users/{id}");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(response.Data, Is.Not.Null);
        Assert.That(response.Data!.Id, Is.EqualTo(id));
        Assert.That(response.Data.Email, Does.Contain("@"));
    }

    [Test]
    public async Task CreateUser_ReturnsCreated()
    {
        var response = await Client.PostAsync<User>("/users", new { name = "Test", username = "tester", email = "t@example.com" });

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
        Assert.That(response.Data, Is.Not.Null);
        Assert.That(response.Data!.Name, Is.EqualTo("Test"));
    }
}