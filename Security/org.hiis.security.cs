using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace org.hiis.Security {
	#region Impersonator
	// http://platinumdogs.me/2008/10/30/net-c-impersonation-with-network-credentials/
	public enum LogonType {
		LOGON32_LOGON_INTERACTIVE = 2,
		LOGON32_LOGON_NETWORK = 3,
		LOGON32_LOGON_BATCH = 4,
		LOGON32_LOGON_SERVICE = 5,
		LOGON32_LOGON_UNLOCK = 7,
		LOGON32_LOGON_NETWORK_CLEARTEXT = 8, // Win2K or higher
		LOGON32_LOGON_NEW_CREDENTIALS = 9 // Win2K or higher
	}
	public enum LogonProvider {
		LOGON32_PROVIDER_DEFAULT = 0,
		LOGON32_PROVIDER_WINNT35 = 1,
		LOGON32_PROVIDER_WINNT40 = 2,
		LOGON32_PROVIDER_WINNT50 = 3
	}
	public enum ImpersonationLevel {
		SecurityAnonymous = 0,
		SecurityIdentification = 1,
		SecurityImpersonation = 2,
		SecurityDelegation = 3
	}
	class Win32NativeMethods {
		[DllImport("advapi32.dll", SetLastError = true)]
		public static extern int LogonUser(string lpszUserName,
				 string lpszDomain,
				 string lpszPassword,
				 int dwLogonType,
				 int dwLogonProvider,
				 ref IntPtr phToken);

		[DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int DuplicateToken(IntPtr hToken,
					int impersonationLevel,
					ref IntPtr hNewToken);

		[DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool RevertToSelf();

		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		public static extern bool CloseHandle(IntPtr handle);
	}
	public class Impersonator : IDisposable {
		private WindowsImpersonationContext _wic;
		/// <summary>
		/// Begins impersonation with the given credentials, Logon type and Logon provider.
		/// </summary>
		/// <param name = "userName" > Name of the user.</param>
		/// <param name = "domainName" > Name of the domain.</param>
		/// <param name = "password" > The password. <see cref = "System.String" /></ param >
		/// < param name="logonType">Type of the logon.</param>
		/// <param name = "logonProvider" > The logon provider. <see cref = "Mit.Sharepoint.WebParts.EventLogQuery.Network.LogonProvider" /></ param >
		public Impersonator(string userName, string domainName, string password, LogonType logonType, LogonProvider logonProvider) {
			Impersonate(userName, domainName, password, logonType, logonProvider);
		}

		/// <summary>
		/// Begins impersonation with the given credentials.
		/// </summary>
		/// <param name = "userName" > Name of the user.</param>
		/// <param name = "domainName" > Name of the domain.</param>
		/// <param name = "password" > The password. <see cref = "System.String" /></ param >
		public Impersonator(string userName, string domainName, string password) {
			Impersonate(userName, domainName, password, LogonType.LOGON32_LOGON_INTERACTIVE, LogonProvider.LOGON32_PROVIDER_DEFAULT);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Impersonator"/> class.
		/// </summary>
		public Impersonator() { }

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose() {
			UndoImpersonation();
		}

		/// <summary>
		/// Impersonates the specified user account.
		/// </summary>
		/// <param name = "userName" > Name of the user.</param>
		/// <param name = "domainName" > Name of the domain.</param>
		/// <param name = "password" > The password. <see cref = "System.String" /></ param >
		public void Impersonate(string userName, string domainName, string password) {
			Impersonate(userName, domainName, password, LogonType.LOGON32_LOGON_INTERACTIVE, LogonProvider.LOGON32_PROVIDER_DEFAULT);
		}
		/// <summary>
		/// Impersonates the specified user account.
		/// </summary>
		/// <param name = "userName" > Name of the user.</param>
		/// <param name = "domainName" > Name of the domain.</param>
		/// <param name = "password" > The password. <see cref = "System.String" /></ param >
		/// < param name="logonType">Type of the logon.</param>
		/// <param name = "logonProvider" > The logon provider. <see cref = "Mit.Sharepoint.WebParts.EventLogQuery.Network.LogonProvider" /></ param >
		public void Impersonate(string userName, string domainName, string password, LogonType logonType, LogonProvider logonProvider) {
			UndoImpersonation();

			IntPtr logonToken = IntPtr.Zero;
			IntPtr logonTokenDuplicate = IntPtr.Zero;
			try {
				// revert to the application pool identity, saving the identity of the current requestor
				_wic = WindowsIdentity.Impersonate(IntPtr.Zero);

				// do logon & impersonate
				if (Win32NativeMethods.LogonUser(userName,
						domainName,
						password,
						(int)logonType,
						(int)logonProvider,
						ref logonToken) != 0) {
					if (Win32NativeMethods.DuplicateToken(logonToken, (int)ImpersonationLevel.SecurityImpersonation, ref logonTokenDuplicate) != 0) {
						var wi = new WindowsIdentity(logonTokenDuplicate);
						wi.Impersonate(); // discard the returned identity context (which is the context of the application pool)
					} else
						throw new Win32Exception(Marshal.GetLastWin32Error());
				} else
					throw new Win32Exception(Marshal.GetLastWin32Error());
			} finally {
				if (logonToken != IntPtr.Zero)
					Win32NativeMethods.CloseHandle(logonToken);

				if (logonTokenDuplicate != IntPtr.Zero)
					Win32NativeMethods.CloseHandle(logonTokenDuplicate);
			}
		}

		/// <summary>
		/// Stops impersonation.
		/// </summary>
		private void UndoImpersonation() {
			// restore saved requestor identity
			if (_wic != null)
				_wic.Undo();
			_wic = null;
		}
	}
	#endregion
	#region NetResource
	// http://stackoverflow.com/questions/295538/how-to-provide-user-name-and-password-when-connecting-to-a-network-share/1197430#1197430
	[StructLayout(LayoutKind.Sequential)]
	public class NetResource {
		public ResourceScope Scope;
		public ResourceType ResourceType;
		public ResourceDisplaytype DisplayType;
		public int Usage;
		public string LocalName;
		public string RemoteName;
		public string Comment;
		public string Provider;
	}
	public enum ResourceScope : int {
		Connected = 1,
		GlobalNetwork,
		Remembered,
		Recent,
		Context
	};
	public enum ResourceType : int {
		Any = 0,
		Disk = 1,
		Print = 2,
		Reserved = 8,
	}
	public enum ResourceDisplaytype : int {
		Generic = 0x0,
		Domain = 0x01,
		Server = 0x02,
		Share = 0x03,
		File = 0x04,
		Group = 0x05,
		Network = 0x06,
		Root = 0x07,
		Shareadmin = 0x08,
		Directory = 0x09,
		Tree = 0x0a,
		Ndscontainer = 0x0b
	}
	public class NetworkConnection : IDisposable {
		string _networkName;
		public NetworkConnection(string networkName, System.Net.NetworkCredential credentials) {
			_networkName = networkName;

			var netResource = new NetResource() {
				Scope = ResourceScope.GlobalNetwork,
				ResourceType = ResourceType.Disk,
				DisplayType = ResourceDisplaytype.Share,
				RemoteName = networkName
			};

			var userName = string.IsNullOrEmpty(credentials.Domain)
					? credentials.UserName
					: string.Format(@"{0}\{1}", credentials.Domain, credentials.UserName);

			var result = WNetAddConnection2(
					netResource,
					credentials.Password,
					userName,
					0);

			if (result != 0) {
				throw new Win32Exception(result, "Error connecting to remote share");
			}
		}
		~NetworkConnection() {
			Dispose(false);
		}

		public void Dispose() {
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing) {
			WNetCancelConnection2(_networkName, 0, true);
		}

		[DllImport("mpr.dll")]
		private static extern int WNetAddConnection2(NetResource netResource,
				string password, string username, int flags);
		[DllImport("mpr.dll")]
		private static extern int WNetCancelConnection2(string name, int flags,
				bool force);
	}
	#endregion
}
