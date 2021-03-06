﻿using System;
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
             "按鍵檢測超時"//17
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
