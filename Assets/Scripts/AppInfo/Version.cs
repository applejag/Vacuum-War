using UnityEngine;
using System.Collections;

namespace AppInfo {

	public struct Version {

		public readonly int major;
		public readonly int minor;
		public readonly int build;
		public readonly Label label;
		public string formatted { get { return string.Format("{0} v{1}.{2}.{3}", label.ToString(), major, minor, build); } }

		public Version(Label label, int major, int minor, int build) {
			this.major = major;
			this.minor = minor;
			this.build = build;
			this.label = label;
		}
		
		public static int CompareTo(Version version, Version other) {
			if (version.label != other.label)
				return version.label > other.label ? 1 : -1;

			if (version.major != other.major)
				return version.major > other.major ? 1 : -1;

			if (version.minor != other.minor)
				return version.minor > other.minor ? 1 : -1;

			if (version.build != other.build)
				return version.build > other.build ? 1 : -1;

			return 0;
		}

		public enum Label {
			Prototype,
			Alpha,
			Beta,
			Release
		}
	}

}
