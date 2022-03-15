mergeInto(LibraryManager.library, {
	GetWindowWidth:function()
	{
		return window.screen.availWidth;
	},
	GetWindowHeight:function()
	{
		return window.screen.availHeight;
	},
	ResetCanvasSize:function(width,height)
	{
		document.getElementById("unityContainer").style.width = width + "px";
		document.getElementById("unityContainer").style.height = height + "px";
	},
});