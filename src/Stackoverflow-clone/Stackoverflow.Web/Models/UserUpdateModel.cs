using Autofac;
using Microsoft.AspNetCore.Identity;
using Stackoverflow.Infrastructure.Membership;
using System.ComponentModel.DataAnnotations;
using SixLabors.ImageSharp.PixelFormats;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage;
using Azure.Storage.Sas;

//using Amazon.S3;
//using Amazon.S3.Transfer;
//using Amazon;

namespace Stackoverflow.Web.Models
{
    public class UserUpdateModel
    {
        private ILifetimeScope _scope;

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }

        public string? PhoneNumber { get; set; }

        public string? ProfilePicture { get; set; }

        // New property to handle file upload
        [Display(Name = "Profile Picture")]
        public IFormFile ProfilePictureFile { get; set; }

        [Required]
        public string? Captcha { get; set; }

        public UserUpdateModel() { }

        public async Task<bool> UpdateProfileAsync(UserManager<ApplicationUser> userManager2)
        {
            if (!string.IsNullOrEmpty(ProfilePictureFile?.FileName))
            {
                //DotNetEnv.Env.Load();
                string extension = Path.GetExtension(ProfilePictureFile.FileName);
                string originalFileName = Path.GetFileNameWithoutExtension(ProfilePictureFile.FileName);
                string fileName = $"{Guid.NewGuid()}_{originalFileName}{extension}";
                
                //AWS CREDENTIALS

                string storageAccountName = System.Environment.GetEnvironmentVariable("AZURE_STORAGE_ACCOUNT") ?? "default_value";
                string storageAccountKey = System.Environment.GetEnvironmentVariable("AZURE_STORAGE_KEY") ?? "default_value";
                string containerName = System.Environment.GetEnvironmentVariable("AZURE_STORAGE_CONTAINER") ?? "profilepictures";
                // create a container for profile pictures

                BlobServiceClient blobServiceClient = new BlobServiceClient(new Uri($"https://{storageAccountName}.blob.core.windows.net/"), new StorageSharedKeyCredential(storageAccountName, storageAccountKey));

                BlobContainerClient blobClient = blobServiceClient.GetBlobContainerClient(containerName);
                
                // Create the container if it does not exist
                if (blobClient.Exists() == false)
                {  
                    await blobClient.CreateIfNotExistsAsync();
                }

                try
                {

                    // Get a reference to the blob
                    //BlobClient blobClient = blobServiceClient.GetBlobClient(containerName, fileName);

                    // Generate a SAS token with read permissions
                    BlobSasBuilder sasBuilder = new BlobSasBuilder
                    {
                        BlobContainerName = containerName,
                        BlobName = fileName,
                        Resource = "b",
                        StartsOn = DateTimeOffset.UtcNow,
                        ExpiresOn = DateTimeOffset.UtcNow.AddHours(1) // adjust the expiration time as needed
                    };

                    sasBuilder.SetPermissions(BlobSasPermissions.Read);

                    string sasToken = sasBuilder.ToSasQueryParameters(new StorageSharedKeyCredential(storageAccountName, storageAccountKey)).ToString();

                    // Construct the URL with the SAS token
                    string blobUrl = $"{blobClient.Uri.AbsoluteUri}?{sasToken}";

                    // Store the Azure Blob Storage URL in ProfilePicture
                    ProfilePicture = blobUrl;

                    //BlobClient blobClient = containerClient.GetBlobClient(fileName);
                    //await blobClient.UploadAsync(ProfilePictureFile.OpenReadStream(), new BlobUploadOptions());


                    //// Set the blob to be publicly accessible
                    //BlobProperties properties = await blobClient.GetPropertiesAsync();
                    //await blobClient.SetAccessTierAsync(AccessTier.Hot);

                    // To make the blob publicly accessible, you need to set the blob's ACL to PublicBlob
                    //await blobClient.SetPermissionsAsync / PublicAccess.Blob);


                    //properties.AccessTier = AccessTier.Hot;
                    //properties.BlobAccessType = BlobAccessType.Blob;
                    //await blobClient.GetPropertiesAsync(properties);

                    // Store the Azure Blob Storage URL in ProfilePicture
                    //ProfilePicture = blobClient.Uri.AbsoluteUri;
                }

                //AWS CREDENTIALS

                //string awsAccessKeyId = System.Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID") ?? "default_value";
                //string awsSecretkey = System.Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY") ?? "default_value";
                //string awsAccessKeyId = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID") ?? "AWS_ACCESS_KEY_ID";
                //string awsSecretkey = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY") ?? "AWS_SECRET_ACCESS_KEY";

                //RegionEndpoint bucketRegion = RegionEndpoint.USEast1;
                //var bucketName = "deborajaspb9";
                //var bucketFolder = "Image";

                //try
                //{

                //    using (var client = new AmazonS3Client(awsAccessKeyId, awsSecretkey, bucketRegion))
                //    {
                //        var transferUtility = new TransferUtility(client);
                //        //await transferUtility.UploadAsync(ProfilePictureFile.OpenReadStream(), "deborajaspb9", fileName);
                //        await transferUtility.UploadAsync(ProfilePictureFile.OpenReadStream(), bucketName, $"{bucketFolder}/{fileName}");
                //    }

                //    // Store the S3 file link in ProfilePicture
                //    ProfilePicture = $"https://{bucketName}.s3.amazonaws.com/{bucketFolder}/{fileName}";
                //    //ProfilePicture = $"https://us-east-1.console.aws.amazon.com/s3/object/deborajaspb9?region=us-east-1&bucketType=general&prefix=files/{fileName}";

                //}
                catch (Exception ex)
                {
                    Console.WriteLine("Error uploading file: " + ex.Message);
                    return false;
                }

            }

            var user = await userManager2.FindByEmailAsync(Email);
            if (user != null)
            {
                user.FirstName = FirstName;
                user.LastName = LastName;
                user.PhoneNumber = PhoneNumber;

                if (!string.IsNullOrEmpty(ProfilePicture))
                {
                    user.ProfilePicture = ProfilePicture;
                }

                var result = await userManager2.UpdateAsync(user);
                return result.Succeeded;
            }

            return false;
        }

        internal async Task LoadAsync(UserManager<ApplicationUser> userManager1, string userId)
        {
            var user = await userManager1.FindByIdAsync(userId);
            if (user != null)
            {
                Email = user.Email;
                FirstName = user.FirstName;
                LastName = user.LastName;
                PhoneNumber = user.PhoneNumber;
                ProfilePicture = user.ProfilePicture;
            }
        }

        internal void Resolve(ILifetimeScope scope)
        {
            _scope = scope;
        }

        internal bool IsImage(IFormFile ProfilePictureFile)
        {
            try
            {
                using (var image = SixLabors.ImageSharp.Image.Load<Rgba32>(ProfilePictureFile.OpenReadStream()))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

    }
}