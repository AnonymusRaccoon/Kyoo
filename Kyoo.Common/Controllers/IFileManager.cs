using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Kyoo.Models;
using Microsoft.AspNetCore.Mvc;

namespace Kyoo.Controllers
{
	/// <summary>
	/// A service to abstract the file system to allow custom file systems (like distant file systems or external providers)
	/// </summary>
	public interface IFileManager
	{
		// TODO find a way to handle Transmux/Transcode with this system.

		/// <summary>
		/// Used for http queries returning a file. This should be used to return local files
		/// or proxy them from a distant server
		/// </summary>
		/// <remarks>
		///	If no file exists at the given path or if the path is null, a NotFoundResult is returned
		/// to handle it gracefully.
		/// </remarks>
		/// <param name="path">The path of the file.</param>
		/// <param name="rangeSupport">
		/// Should the file be downloaded at once or is the client allowed to request only part of the file
		/// </param>
		/// <param name="type">
		/// You can manually specify the content type of your file.
		/// For example you can force a file to be returned as plain text using <c>text/plain</c>.
		/// If the type is not specified, it will be deduced automatically (from the extension or by sniffing the file).
		/// </param>
		/// <returns>An <see cref="IActionResult"/> representing the file returned.</returns>
		public IActionResult FileResult([CanBeNull] string path, bool rangeSupport = false, string type = null);

		/// <summary>
		/// Read a file present at <paramref name="path"/>. The reader can be used in an arbitrary context.
		/// To return files from an http endpoint, use <see cref="FileResult"/>.
		/// </summary>
		/// <param name="path">The path of the file</param>
		/// <exception cref="FileNotFoundException">If the file could not be found.</exception>
		/// <returns>A reader to read the file.</returns>
		public Stream GetReader([NotNull] string path);

		/// <summary>
		/// Create a new file at <paramref name="path"></paramref>.
		/// </summary>
		/// <param name="path">The path of the new file.</param>
		/// <returns>A writer to write to the new file.</returns>
		public Stream NewFile([NotNull] string path);

		/// <summary>
		/// List files in a directory.
		/// </summary>
		/// <param name="path">The path of the directory</param>
		/// <returns>A list of files's path.</returns>
		public Task<ICollection<string>> ListFiles([NotNull] string path);

		/// <summary>
		/// Check if a file exists at the given path.
		/// </summary>
		/// <param name="path">The path to check</param>
		/// <returns>True if the path exists, false otherwise</returns>
		public Task<bool> Exists([NotNull] string path);

		/// <summary>
		/// Get the extra directory of a show.
		/// This method is in this system to allow a filesystem to use a different metadata policy for one.
		/// It can be useful if the filesystem is readonly.
		/// </summary>
		/// <param name="show">The show to proceed</param>
		/// <returns>The extra directory of the show</returns>
		public string GetExtraDirectory(Show show);
		
		/// <summary>
		/// Get the extra directory of a season.
		/// This method is in this system to allow a filesystem to use a different metadata policy for one.
		/// It can be useful if the filesystem is readonly.
		/// </summary>
		/// <param name="season">The season to proceed</param>
		/// <returns>The extra directory of the season</returns>
		public string GetExtraDirectory(Season season);
		
		/// <summary>
		/// Get the extra directory of an episode.
		/// This method is in this system to allow a filesystem to use a different metadata policy for one.
		/// It can be useful if the filesystem is readonly.
		/// </summary>
		/// <param name="episode">The episode to proceed</param>
		/// <returns>The extra directory of the episode</returns>
		public string GetExtraDirectory(Episode episode);
	}
}