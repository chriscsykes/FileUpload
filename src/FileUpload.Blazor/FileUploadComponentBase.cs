using Blazorise;
using FileUpload.Localization;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Components;

namespace FileUpload.Blazor;

public abstract class FileUploadComponentBase : AbpComponentBase
{


    protected FileUploadComponentBase()
    {
        LocalizationResource = typeof(FileUploadResource);
    }

    // WriteToStreamAsync example from https://blazorise.com/docs/components/file
    string fileContent;

    protected async Task OnChanged(FileChangedEventArgs e)
    {
        try
        {
            foreach (var file in e.Files)
            {
                // A stream is going to be the destination stream we're writing to.
                using (var stream = new MemoryStream())
                {
                    // Here we're telling the FileEdit where to write the upload result
                    await file.WriteToStreamAsync(stream);

                    // Once we reach this line it means the file is fully uploaded.
                    // In this case we're going to offset to the beginning of file
                    // so we can read it.
                    stream.Seek(0, SeekOrigin.Begin);

                    // Use the stream reader to read the content of uploaded file,
                    // in this case we can assume it is a textual file.
                    using (var reader = new StreamReader(stream))
                    {
                        fileContent = await reader.ReadToEndAsync();
                    }
                }
            }

        }
        catch (Exception exc)
        {
            Console.WriteLine(exc.Message);
        }
        finally
        {
            this.StateHasChanged();
        }
    }

    protected void OnWritten(FileWrittenEventArgs e)
    {
        Console.WriteLine($"File: {e.File.Name} Position: {e.Position} Data: {Convert.ToBase64String(e.Data)}");
    }

    protected Task OnProgressed(FileProgressedEventArgs e)
    {
        Console.WriteLine($"File: {e.File.Name} Progress: {e.Percentage}");
        return Task.CompletedTask;  

    }

    protected void OnEnded(FileEndedEventArgs e)
    {
        if (e.Success)
        {
            Console.WriteLine($"------------- FINISHED LOADING '{e.File.Name}' -------------------");

        }

    }

    // // OpenReadStream example from https://blazorise.com/docs/components/file
    //const int OneMb = 1024 * 1024;

    //protected async Task OnChanged(FileChangedEventArgs e)
    //{
    //    try
    //    {
    //        var file = e.Files.FirstOrDefault();
    //        if (file == null)
    //        {
    //            return;
    //        }

    //        var buffer = new byte[OneMb];
    //        using (var bufferedStream = new BufferedStream(file.OpenReadStream(long.MaxValue), OneMb))
    //        {
    //            int readCount = 0;
    //            int readBytes;
    //            while ((readBytes = await bufferedStream.ReadAsync(buffer, 0, OneMb)) > 0)
    //            {
    //                Console.WriteLine($"Read:{readCount++} {readBytes / (double)OneMb} MB");
    //                // Do work on the first 1MB of data
    //            }
    //        }
    //    }
    //    catch (Exception exc)
    //    {
    //        Console.WriteLine(exc.Message);
    //    }
    //    finally
    //    {
    //        this.StateHasChanged();
    //    }
    //}

    //protected void OnWritten(FileWrittenEventArgs e)
    //{
    //    Console.WriteLine($"File: {e.File.Name} Position: {e.Position} Data: {Convert.ToBase64String(e.Data)}");
    //}

    //protected void OnProgressed(FileProgressedEventArgs e)
    //{
    //    Console.WriteLine($"File: {e.File.Name} Progress: {e.Percentage}");
    //}

    //protected void OnEnded(FileEndedEventArgs e)
    //{
    //    if (e.Success)
    //    {
    //        Console.WriteLine($"------------- FINISHED LOADING '{e.File.Name}' -------------------");

    //    }

    //}
}