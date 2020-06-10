function pageLoad ()
{
    $("#divScriptBody input").on("blur", function() {
        var $input = $(this);
        var name = $input.attr("name");
        var value = $input.val();
        
        $("#divScriptBody input[name=' + name + ']").val(value);
    });
}
