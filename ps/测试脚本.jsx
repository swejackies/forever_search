var dodo = function (info)
{
    app.documents.add();// �½�һ���ĵ�
}

dodo();

// Create a color to be used with the fill command
//alert("[fill][begin]");
var colorRef = new SolidColor
colorRef.rgb.red = 255
colorRef.rgb.green = 100
colorRef.rgb.blue = 0
// Now apply fill to the current selection
app.activeDocument.selection.fill(colorRef)
//alert("[fill][end]");

//alert("[alert][begin]");
if( BridgeTalk.appName == "photoshop" ) {
	alert("photoshop.����");
}else{
	alert("non photoshop����");
}
//alert("[alert][end]");