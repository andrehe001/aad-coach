$(window).on("load", function() {    

	var theWindow        = $(window),
        $content         = $(".content-container"),
        $headline         = $("h1"),
	    aspectRatio      = 1920 / 1080;
            
    function resizeMargin() {

        var originalValue = 110;
        if ( (theWindow.width() / theWindow.height()) < aspectRatio ) {
            var marginTop = theWindow.height() / 1080 * originalValue;
            $content.css('margin-top', marginTop)
            return marginTop;
        } else {
            var heightIfRatioMet = theWindow.width() / 1920 * 1080;
            var truncatedHeight = heightIfRatioMet - theWindow.height();
            var truncatedTop = truncatedHeight / 2;
            var marginTop = theWindow.width() / 1920 * originalValue - truncatedTop;
            $content.css('margin-top', Math.max(0, marginTop));
            return marginTop;
        }

    }

    function resizeHeight(marginTop) {

        var originalValue = 860;
        if ( (theWindow.width() / theWindow.height()) < aspectRatio ) {
            var newHeight = theWindow.height() / 1080 * originalValue;
            $content.css('height', newHeight - $headline.height())
        } else {
            var newHeight = theWindow.width() / 1920 * originalValue;
            $content.css('height', Math.min(theWindow.height() - marginTop - $headline.height(), newHeight - $headline.height()))
        }

    }

    function resizeWidth(marginTop) {

        var originalValue = 930;
        if ( (theWindow.width() / theWindow.height()) < aspectRatio ) {
            var newWidth = theWindow.height() / 1080 * originalValue;
            $content.css('width', Math.min(theWindow.width(), newWidth))
        } else {
            $content.css('width', theWindow.width() / 1920 * originalValue)
        }

    }

	function resizeBg() {
        var marginTop = resizeMargin();
        resizeHeight(marginTop);
        resizeWidth();
	}
	                   			
	theWindow.resize(resizeBg).trigger("resize").scrollTop();

});