using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace thinger.RTUModbus
{
    /// <summary>
    /// 构造方法
    /// </summary>
    public class Modbus
    {
        #region 构造方法
        public ModbusRTU()
        {
            serialPort = new SerialPort();
        }
        #endregion
        #region 字段或者属性
        /// <summary>
        /// 读取超时时间，单位毫秒
        /// </summary>
        public int readTimeout { get; set; } = 2000;
        /// <summary>
        /// 写入超时时间，单位毫秒
        /// </summary>
        public int writeTimeout { get; set; } = 2000;
        #endregion
        #region 建立连接
        private SerialPort serialPort;
        /// <summary>
        /// 连接串口
        /// </summary>
        /// <param name="portName">串口号</param>
        /// <param name="baudRate">波特率</param>
        /// <param name="parity">奇偶校验</param>
        /// <param name="dataBits">数据位</param>
        /// <param name="stopBits">停止位</param>
        /// <returns></returns>
        public bool Connect(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits)
        {
            // 如果串口已经打开，先关闭串口
            if (serialPort != null && serialPort.IsOpen)
            {
                serialPort.Close();
            }
            // 设置串口参数
            serialPort.PortName = portName;
            serialPort.BaudRate = baudRate;
            serialPort.Parity = parity;
            serialPort.DataBits = dataBits;
            serialPort.StopBits = stopBits;

            serialPort.ReadTimeout = this.readTimeout;
            serialPort.WriteTimeout = this.writeTimeout;

            // 打开串口
            try
            {

                serialPort.Open();

            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion
        #region 01H读取输出线圈
        /// <summary>
        /// 01H读取输出线圈
        /// </summary>
        /// <param name="slaveld">站地址</param>
        /// <param name="startAddress">起始地址</param>
        /// <param name="numberOfPoints">点数</param>
        /// <returns>返回数据</returns>
        public byte[] ReadOutputCoils(byte slaveld, ushort startAddress, ushort numberOfPoints)
        {
            // 拼接报文
            // 发送报文
            // 接收报文
            // 验证报文
            // 解析报文
        }
        #endregion
        #region Crc16校验

        /// <summary>
        /// Modbus RTU CRC16 校验工具（逐位计算法，代码精简）
        /// </summary>
        public static class Crc16ModbusBitwise
        {
            private const ushort Polynomial = 0xA001;
            private const ushort InitialValue = 0xFFFF;

            /// <summary>
            /// 计算CRC16值
            /// </summary>
            public static ushort ComputeCrc(byte[] data)
            {
                if (data == null)
                    throw new ArgumentNullException(nameof(data));

                ushort crc = InitialValue;
                foreach (byte b in data)
                {
                    crc ^= b;
                    for (int i = 0; i < 8; i++)
                    {
                        if ((crc & 0x0001) != 0)
                            crc = (ushort)((crc >> 1) ^ Polynomial);
                        else
                            crc >>= 1;
                    }
                }
                return crc;
            }

            /// <summary>
            /// 返回Modbus格式的CRC字节数组（低前高后）
            /// </summary>
            public static byte[] ComputeCrcBytes(byte[] data)
            {
                ushort crc = ComputeCrc(data);
                return new byte[] { (byte)(crc & 0xFF), (byte)((crc >> 8) & 0xFF) };
            }

        #endregion
        }
    }
}

