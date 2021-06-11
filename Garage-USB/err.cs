using System;
using System.Collections.Generic;
using System.Text;

namespace Garage_USB
{
    public class err
    {
        public int[] BIN_CODE;
        public string[] ERR_MESSAGE =
        {
             "0",//0
             "未下載或無響應",//1
             "激活失敗",//2
             "電流異常",//3
             "校正失敗",//4
             "下載無響應",//5
             "未檢測到電流",//6
             "連接失敗",//7
             "讀取信息失敗",//8
             "讀取配置失敗",//9
             "讀取圖像失敗",//10
             "讀取指紋圖失敗",//11
             "測試指紋圖失敗",//12
             "信號質量過低",//13
             "圖像質量過低",//14
             "控制信息失敗",//15
             "異常圖像",//16
             "按鍵檢測超時",//17
             "唤醒IO常高",//18
             "唤醒检测超时2",//19
             "按压检测超时",//20
             "版本错误",//21
             "通讯错误",//22
             "设置低功耗",//23
             "上电错误",//24
             "取随机数失败",//25
             "写入序列号失败",//26
             "激活key访问失败",//27
             "激活次数用尽",//28
             "治具串口数量异常",//29
             "治具串口访问异常",//30
             "模组RX通讯异常",//31
             "待机电流过大",//32
             "工作电流过大",//33
             "清除失败",//34
             "下载完成后通讯失败",//35
             "Check检测失败"//36

        };
        public err()
        {
            BIN_CODE = new int[64];
            for (int i = 0; i < 64; i++)
                BIN_CODE[i] = i;

        }
        public string message(int code)
        {
            return ERR_MESSAGE[code];
        }
    }
}
