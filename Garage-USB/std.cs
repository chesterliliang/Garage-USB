using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Linq;

namespace Garage_USB
{
    public class std
    {
        private byte[] header = { 0xef, 0x01 };//2
        private byte[] add_ff = { 0xff, 0xff, 0xff, 0xff };//4

        private byte[] get_crc(byte[] data, int len)
        {
            int i_sum = 0;
            for(int i=0;i<len;i++)
            {
                i_sum += data[i];
            }
            byte[] sum = new byte[2];
            i_sum = 0xffff & i_sum;
            sum[0] = (byte)(i_sum / 256);
            sum[1] = (byte)(i_sum - (i_sum / 256) * 256);
            return sum;
        }

        public byte[] get_cmd(byte pkg_id, int len, byte[] data)
        {
            byte[] cmd = new byte[2 + 4 + 1 + 2 + len + 2];
            Buffer.BlockCopy(header, 0, cmd, 0, 2);
            Buffer.BlockCopy(add_ff, 0, cmd, 2, 4);
            cmd[6] = pkg_id;
            cmd[7] = (byte)( (len+2) / 256);
            cmd[8] = (byte)((len+2) - ((len+2) / 256) * 256);
            if(len!=0)
                Buffer.BlockCopy(data, 0, cmd, 9, len);
            Buffer.BlockCopy(get_crc(cmd.Skip(6).Take(3+len).ToArray(),3+len), 0, cmd, 9+len, 2);
            return cmd;
        }

        public int get_result(byte[] cmd, int len, ref byte[] data, ref int data_len)
        {
            byte[] inner_cmd = new byte[len];
            int inner_len = 0;
            if(cmd[0]==0x55)
            {
                inner_len = len - 1;
                Buffer.BlockCopy(cmd, 1, inner_cmd, 0, len - 1);
            }
            else
            {
                inner_len = len;
                Buffer.BlockCopy(cmd, 0, inner_cmd, 0, len);
            }
            if(inner_cmd[0]!=0xef|| inner_cmd[1]!=1)
                return def.RTN_FAIL;
            if (inner_cmd[7] == 0 && inner_cmd[8] == 0)
                return def.RTN_FAIL;
            if(inner_cmd[9]!=0)
                return inner_cmd[9];
            data_len = inner_cmd[7] * 256 + inner_cmd[8] - 2 -1;
            Buffer.BlockCopy(inner_cmd.Skip(inner_len-data_len-2).Take(data_len).ToArray(), 0, data, 0, data_len);
            return def.RTN_OK;
        }

    }
}
