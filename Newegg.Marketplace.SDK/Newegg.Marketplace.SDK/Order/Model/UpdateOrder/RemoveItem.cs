﻿/**
Newegg Marketplace SDK Copyright © 2000-present Newegg Inc. 

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
documentation files (the “Software”), to deal in the Software without restriction, including without limitation the 
rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software,
and to permit persons to whom the Software is furnished to do so, subject to the following conditions: 
The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software. 
THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR 
PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS 
BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, 
TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE
OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
**/

using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

using Newtonsoft.Json;

using Newegg.Marketplace.SDK.Model;
using Newegg.Marketplace.SDK.Base.Util;

namespace Newegg.Marketplace.SDK.Order.Model
{
    [XmlRoot("NeweggAPIRequest")]
    public class RemoveItemRequest : RequestModel<RemoveItemRequestBody>
    {
        public RemoveItemRequest()
        {
            OperationType = "KillItemRequest";
        }
        public RemoveItemRequest(string[] SellerPartNumbers, string memo = null)
        {
            OperationType = "KillItemRequest";
            RequestBody = new RemoveItemRequestBody()
            {
                KillItem = new RemoveItemRequestBody.KillItemInfo()
                {
                    Order = new RemoveItemRequestBody.KillItemOrder()
                    {
                        ItemList = new List<Item>(),
                        Memo = memo
                    }
                }
            };
            foreach (string SellerPartNumber in SellerPartNumbers)
                RequestBody.KillItem.Order.ItemList.Add(new Item(SellerPartNumber));
        }
    }
    public class RemoveItemRequestBody
    {
        public KillItemInfo KillItem { set; get; }
        public class KillItemInfo
        {
            public KillItemOrder Order { get; set; }
        }
        public class KillItemOrder
        {
            [XmlArrayItem("Item"), JsonConverter(typeof(JsonMoreLevelSeConverter), "Item")]
            public List<Item> ItemList { get; set; }
            public string Memo { get; set; }
        }
    }

    [XmlRoot(ElementName = "NeweggAPIResponse")]
    public class RemoveItemResponse : ResponseModel<RemoveItemResponseBody>
    {
    }
    public class RemoveItemResponseBody
    {
        public string RequestDate { get; set; }
        public KillItemOrderResponse Orders { get; set; }

        public class KillItemOrderResponse
        {
            public string OrderNumber { set; get; }
            public ResponseResult Result { set; get; }

            public class ResponseResult
            {
                public List<Item> ItemList { set; get; }
            }
        }
    }

    public class Item
    {
        public Item() { }
        public Item(string sellerPartNumber)
        {
            SellerPartNumber = sellerPartNumber;
        }
        [XmlIgnore]
        public string SellerPartNumber { get; set; }
        [XmlElement("SellerPartNumber"), JsonIgnore]
        public XmlNode CDATASellerPartNumber
        {
            get
            {
                if (string.IsNullOrEmpty(SellerPartNumber))
                    return null;
                return new XmlDocument().CreateCDataSection(SellerPartNumber);
            }
            set { SellerPartNumber = value.Value; }
        }
    }
}
