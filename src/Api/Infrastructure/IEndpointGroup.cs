namespace RentalSystem.Api.Infrastructure;

/// <summary>
/// RoutePrefix defines the group of route name, and it should be a string. 
/// In default it is null when nothing was set. And when null it is auto
/// set to $"/api/{ClassName}" in default <see cref="WebApplicationExtensions"/>.

/// In a endpoint implementation, who must inherit this interface as an
/// development rule you must implement Map method. As abstract method
/// is an incomplete method.

/// RouteGroupBuilder allows you to build relative api within the same 
/// group and provide you POST/GET/PATCH/PUT/DELETE http methods
/// to define your endpoint characteristic on their action. 
/// </summary>
public interface IEndpointGroup
{
    static virtual string? RoutePrefix => null;

    static abstract void Map(RouteGroupBuilder routeGroupBuilder);
}