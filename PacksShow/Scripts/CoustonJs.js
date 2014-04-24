function tdclick() {
    //0.保存当前的td节点  
    var td = $(this);
    //1.取出当前td中的文本内容保存起来  
    var text = td.text();
    //2.清空td里面的内容  
    td.html("");  //也可以用td.empty();  
    //3.建立一个文本框，也就是input的元素节点
    var id = td.attr("id");
    var inputHtml = "<input name='" + id + "_hidden' style='width:50px'>";
    var input = $(inputHtml);
    //if (name == "Platform")
    //{
    //    input = $("<input style='width:60px'>");
    //}
    //4.设置文本框的值是保存起来的文本内容  
    input.attr("value", text);
    //5.将文本框加入到td中  
    td.append(input);  //也可以用input.appendTo(td)  
    //5.5让文本框里面的文字被高亮选中  
    //需要将jquery的对象转换成dom对象  
    var inputdom = input.get(0);
    inputdom.select();
    //6.需要清除td上的点击事件  
    td.unbind("click");

    //当文本框失去焦点后，将修改后的值保存到隐藏控件中。并将单元格的内容变回原来的内容。
    $("input").blur(function () {
        var inputnode = $(this);
        //2.保存当前文本框的内容  
        var intputext = inputnode.val();
        //3.清空td里面的内容  
        //var tdNode = inputnode.parent();
        //4.将保存的文本框的内容填充到td中  
        //tdNode.html(intputext);
        //获得隐藏input的id
        //var idHidden = id + "_hidden";
        ////将文本框内容赋值给隐藏input
        //$("#" + idHidden).val("aaaa");
        input.attr("value", intputext);
        //inputHidden.attr("value", intputext);
        ////5.让td重新拥有点击事件  
        //tdNode.dblclick(tdclick);
    });
}