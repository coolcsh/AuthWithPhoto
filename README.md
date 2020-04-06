# AuthWithPhoto

This project shows you how to take an ASP.NET Core 3.1 Web Application with authentication and let users add a photo to their profile. It shows how to customize the IdentityUser in this case with the new ApplicationUser that adds the property for the photo. It also has scaffoldeded the Index page of the account management so it can be customized, in this case to add support for uploading the photo. And there is a controller that returns the current photo from the data and the LoginPartial has been updated to display the photo.

So this highlights:
- Extending IdentityUser
- Scaffolding account management for modification
- Controller that returns and image
