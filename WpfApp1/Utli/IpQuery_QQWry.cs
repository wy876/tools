using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Utli
{
    public class IpQuery_QQWry
    {
		public IpQuery_QQWry(string _file)
		{
			m_bytes = File.ReadAllBytes(_file);
			m_index_head = read_item_uint32(0);
			m_index_tail = read_item_uint32(4);
		}

		public (string, string, string) find_info(string _ip)
		{
			var _ipitem = _ip.Split('.');
			uint _nip = (Convert.ToUInt32(_ipitem[0]) << 24) | (Convert.ToUInt32(_ipitem[1]) << 16);
			_nip |= (Convert.ToUInt32(_ipitem[2]) << 8) | Convert.ToUInt32(_ipitem[3]);
			var _head = m_index_head;
			var _tail = m_index_tail;
			while (_tail > _head)
			{
				uint _cur = ((_tail - _head) / 7) >> 1;
				_cur = _head + (_cur == 0 ? 1 : _cur) * 7;
				uint _ip_min = read_item_uint32(_cur);
				uint _pos = read_item_uint24(_cur + 4);
				uint _ip_max = read_item_uint32(_pos);
				if (_nip < _ip_min)
				{
					_tail = _cur;
				}
				else if (_nip > _ip_max)
				{
					_head = _cur;
				}
				else
				{
					_pos += 4;
					byte _mode = m_bytes[_pos];
					string _info0, _info1, _desp = "";
					uint _size;
					if (_mode == 0x01)
					{
						uint _main_offset = read_item_uint24(_pos + 1);
						if (m_bytes[_main_offset] == 0x02)
						{
							(_info0, _info1, _) = read_infos(_main_offset, _pos + 8);
							_desp = read_area(_main_offset + 4);
						}
						else
						{
							(_info0, _info1, _size) = read_infos(_main_offset);
							_desp = read_area(_main_offset + _size + 1);
						}
					}
					else if (_mode == 0x02)
					{
						uint _main_offset = read_item_uint24(_pos + 1);
						(_info0, _info1, _) = read_infos(_main_offset, _pos + 4);
						//_desp = read_area (_pos + 8);
					}
					else
					{
						(_info0, _info1, _size) = read_infos(_pos);
						_desp = read_area(_pos + _size + 1);
					}
					return (_info0, _info1, _desp);
				}
			}
			return ("", "", "");
		}

		private (string, string, uint) read_infos(uint _main_pos, uint _sub_pos = 0)
		{
			var (_main_info, _len) = read_item_string(_main_pos);
			_sub_pos = (_sub_pos == 0 ? (_main_pos + (uint)Encoding.GetEncoding("GB2312").GetByteCount(_main_info)) : _sub_pos);
			string _sub_info = read_item_string(_sub_pos).Item1;
			return (_main_info, _sub_info, _len);
		}

		private string read_area(uint _area_pos)
		{
			uint _mode = m_bytes[_area_pos];
			if (_mode == 1 || _mode == 2)
			{
				_area_pos = read_item_uint24(_area_pos + 1);
				if (_area_pos == 0)
				{
					return "";
				}
				else
				{
					return read_item_string(_area_pos).Item1;
				}
			}
			else
			{
				return read_item_string(_area_pos).Item1;
			}
		}

		private uint read_item_uint24(uint _pos)
		{
			uint _n = 0xff & (uint)m_bytes[_pos];
			_n += (0xff & (uint)m_bytes[_pos + 1]) << 8;
			_n += (0xff & (uint)m_bytes[_pos + 2]) << 16;
			return _n;
		}

		private uint read_item_uint32(uint _pos)
		{
			uint _n = 0xff & (uint)m_bytes[_pos];
			_n += (0xff & (uint)m_bytes[_pos + 1]) << 8;
			_n += (0xff & (uint)m_bytes[_pos + 2]) << 16;
			_n += (0xff & (uint)m_bytes[_pos + 3]) << 24;
			return _n;
		}

		private (string, uint) read_item_string(uint _pos)
		{
			List<byte> _l = new List<byte>();
			if (m_bytes[_pos] == 0x01 || m_bytes[_pos] == 0x02)
			{
				_pos = read_item_uint24(_pos + 1);
				if (m_bytes[_pos] >= 0x00 && m_bytes[_pos] <= 0x02)
					return ("", 0);
			}
			while (m_bytes[_pos] != 0x00)
			{
				_l.Add(m_bytes[_pos++]);
			}
			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
			return (Encoding.GetEncoding("GB2312").GetString(_l.ToArray()), (uint)_l.Count);
		}

		//private (T, int) read_item<T> (uint _pos, bool _uint_24 = false) {
		//	if (typeof (T) == typeof (uint)) {
		//		uint _n = 0xff & (uint) m_bytes [_pos];
		//		_n += (0xff & (uint) m_bytes [_pos + 1]) << 8;
		//		_n += (0xff & (uint) m_bytes [_pos + 2]) << 16;
		//		if (!_uint_24) {
		//			_n += (0xff & (uint) m_bytes[_pos + 3]) << 24;
		//			return ((T) (object) _n, 3);
		//		} else {
		//			return ((T) (object) _n, 4);
		//		}
		//	} else if (typeof (T) == typeof (string)) {
		//		List<byte> _l = new List<byte> ();
		//		if (m_bytes [_pos] == 0x01 || m_bytes [_pos] == 0x02) {
		//			(_pos, _) = read_item<uint> (_pos + 1, true);
		//			if (m_bytes [_pos] >= 0x00 && m_bytes [_pos] <= 0x02)
		//				return ((T) (object) "", 0);
		//		}
		//		while (m_bytes [_pos] != 0x00) {
		//			_l.Add (m_bytes [_pos++]);
		//		}
		//		return ((T) (object) Encoding.GetEncoding ("GB2312").GetString (_l.ToArray ()), _l.Count);
		//	}
		//	throw new Exception ("暂不支持T类型");
		//}

		private byte[] m_bytes = null;
		private uint m_index_head = 0;
		private uint m_index_tail = 0;


	}
}
