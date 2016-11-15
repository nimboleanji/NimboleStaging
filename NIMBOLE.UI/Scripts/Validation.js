var errorDisplayType = 1;  /* 1 = text 2 = image 3 = both*/
var imgWrong = "<img src='images/close.png' style='padding-left:4px;' align='absmiddle' /> "; // change it 
var imgCorrect = "<img src='images/correct.png' style='padding-left:4px;' align='absmiddle' /> ";
var color = "red";

var val;

var errormessages = {
    "isrequired": "This field is mandatory.",
    "isnumeric": "Numerics only.",
    "iszipcode":"Numers only.",
    "islowercase": "All lowercase alphabets.",
    "isuppercase": "All uppercase alphabets.",
    "isalphanumeric": "Alpha numerics only",
    "isemail": "Valid email address",
    "isdate": "Select valid date",
    "isinnumericrange": "Value must be in specified range",
    "ismoney": "Enter money format"
};
var texttypes = {
    "numeric": "0123456789",
    //"commanumeric": "0123456789,",
    "username": "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ",
    "characters": "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ ",
    "alphanumeric": "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ ",
    "allcharacters": "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ.~!@#$%^&*()}{|?><+-/ ",
    "specialcharacter": "~!@#$%^&*()}{|?><+-/",
    "yearrange":"0123456789-",
    "email": "email",
    "date": "date",
    "url": "url",
    "compare": "compare",
    "money": "money"
};

function gup(name) {
    name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
    var regexS = "[\\?&]" + name + "=([^&#]*)";
    var regex = new RegExp(regexS);
    var results = regex.exec(window.location.href);
    if (results == null)
        return "";
    else
        return results[1];
}

function checkallcall() {
    $("#checkall").click(function () {
        var checked_status = this.checked;
        $("input[type=checkbox]").each(function () {
            this.checked = checked_status;
        });
    });
}

function validateControl(controlid, isrequired, texttype, min, max) {
    var returnValues = true;
    color = "red";

    //restControls();
    var controlVal = $("#" + controlid).val();
    if (controlid) {
        if (isrequired == "true") {
            if (controlVal == '') {
                showError(controlid, errormessages.isrequired, errorDisplayType, texttype);
                return false;
            }
            else {
                $("#" + controlid).removeAttr("style");
            }
        }
        else {
            if (controlVal == '') {
                $("#" + controlid).removeAttr("style");
            }
            else {
                color = "orange";
            }
        }
        if (controlVal.length != 0) {
            switch (texttype)
            {
                case "email":
                    //var reg = /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/;
                    var reg = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
                    if (reg.test(controlVal) == false) {
                        showError(controlid, errormessages.isemail, errorDisplayType, texttype);
                        returnValues = false;
                    }
                    else {
                        $("#" + controlid).removeAttr("style");
                    }
                    break;
                case "date":
                    var dateRegEx = new RegExp(/^(?:(?:(?:0?[13578]|1[02])(\/|-)31)|(?:(?:0?[1,3-9]|1[0-2])(\/|-)(?:29|30)))(\/|-)(?:[1-9]\d\d\d|\d[1-9]\d\d|\d\d[1-9]\d|\d\d\d[1-9])$|^(?:(?:0?[1-9]|1[0-2])(\/|-)(?:0?[1-9]|1\d|2[0-8]))(\/|-)(?:[1-9]\d\d\d|\d[1-9]\d\d|\d\d[1-9]\d|\d\d\d[1-9])$|^(0?2(\/|-)29)(\/|-)(?:(?:0[48]00|[13579][26]00|[2468][048]00)|(?:\d\d)?(?:0[48]|[2468][048]|[13579][26]))$/);
                    if (dateRegEx.test(controlVal) == false) {
                        showError(controlid, errormessages.isdate, errorDisplayType, texttype);
                        returnValues = false;
                    }
                    else {
                        $("#" + controlid).removeAttr("style");
                    }
                    break;
                case "alphanumeric":
                    if (checkString(controlVal, texttypes.alphanumeric) == true) {
                        showError(controlid, "Enter valid alphanumerics.", errorDisplayType, texttype);
                        returnValues = false;
                    }
                    else {
                        $("#" + controlid).removeAttr("style");
                    }
                    break;
                case "characters":
                    if (checkString(controlVal, texttypes.characters) == true) {
                        showError(controlid, "Enter valid characters.", errorDisplayType, texttype);
                        returnValues = false;
                    }
                    else {
                        $("#" + controlid).removeAttr("style");
                    }
                    break;
                case "yearrange":
                    var number = new RegExp(/^[0-9]{4}(-)+[0-9]{4}$/);
                    if (number.test(controlVal) == false) {
                        showError(controlid, errormessages.isnumeric, errorDisplayType, texttype);
                        returnValues = false;
                    }
                    else {
                        $("#" + controlid).removeAttr("style");
                    }
                    break;
                case "numeric":
                    var number = new RegExp(/^[0-9]*$/);
                    if (number.test(controlVal) == false) {
                        showError(controlid, errormessages.isnumeric, errorDisplayType, texttype);
                        returnValues = false;
                    }
                    else {
                        $("#" + controlid).removeAttr("style");
                    }
                    break;
                case "zipcode":
                    var zipcode = new RegExp(/^\d{6}$/);
                    if (zipcode.test(controlVal) == false) {
                        showError(controlid, errormessages.iszipcode, errorDisplayType, texttype);
                        returnValues = false;
                    }
                    else {
                        $("#" + controlid).removeAttr("style");
                    }
                    //if (checkString(controlVal, texttypes.numeric) == true) {
                    //    showError(controlid, "Enter valid numerics.", errorDisplayType, texttype);
                    //    returnValues = false;
                    //}
                    //else {
                    //    $("#" + controlid).removeAttr("style");
                    //}
                    break;
                case "mobile":
                    var mobile = new RegExp(/^(\+\d{1,3}[- ]?)?\d{10}$/);
                    if (mobile.test(controlVal) == false) {
                        showError(controlid, errormessages.ismobile, errorDisplayType, texttype);
                        returnValues = false;
                    }
                    else {
                        $("#" + controlid).removeAttr("style");
                    }
                    break;
                case "money":
                    var moneytype = new RegExp(/^\$?\d+(\.(\d{2}))?$/);
                    if (moneytype.test(controlVal) == false) {
                        showError(controlid, errormessages.ismoney, errorDisplayType, texttype);
                        returnValues = false;
                    }
                    else {
                        $("#" + controlid).removeAttr("style");
                    }
                    break;
                case "url":
                    if (controlVal.length == 0) { returnValues = true; }

                    // if user has not entered http:// https:// or ftp:// assume they mean http://
                    if (!/^(https?|ftp):\/\//i.test(controlVal)) {
                        val = 'http://' + controlVal; // set both the value
                        $("#" + controlid).val(controlVal); // also update the form element
                    }
                    else {
                        $("#" + controlid).removeAttr("style");
                    }
                    //  var returs = /^(https?|ftp):\/\/(((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&amp;'\(\)\*\+,;=]|:)*@)?(((\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5]))|((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?)(:\d*)?)(\/((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&amp;'\(\)\*\+,;=]|:|@)+(\/(([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&amp;'\(\)\*\+,;=]|:|@)*)*)?)?(\?((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&amp;'\(\)\*\+,;=]|:|@)|[\uE000-\uF8FF]|\/|\?)*)?(\#((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&amp;'\(\)\*\+,;=]|:|@)|\/|\?)*)?$/i.test(val);

                    var urlregex = /^(https?|ftp):\/\/([a-zA-Z0-9.-]+(:[a-zA-Z0-9.&%$-]+)*@)*((25[0-5]|2[0-4][0-9]|1[0-9]{2}|[1-9][0-9]?)(\.(25[0-5]|2[0-4][0-9]|1[0-9]{2}|[1-9]?[0-9])){3}|([a-zA-Z0-9-]+\.)*[a-zA-Z0-9-]+\.(com|edu|gov|int|mil|net|org|biz|arpa|info|name|pro|aero|coop|museum|[a-zA-Z]{2}))(:[0-9]+)*(\/($|[a-zA-Z0-9.,?'\\+&%$#=~_-]+))*$/i.test(val);
                   // return urlregex.test(textval);
                    if (urlregex == false) {
                        showError(controlid, "Enter valid url.", errorDisplayType, texttype);
                        returnValues = false;
                    }
                    else {
                        $("#" + controlid).removeAttr("style");
                    }
                    break;
                case "select":
                    if (controlVal == "") {
                        //showError(controlid, "Please select.", errorDisplayType, texttype);
                        showError(controlid, errormessages.isrequired, errorDisplayType, texttype);
                        returnValues = false;
                    }
                    else {
                        //$("#" + controlid).removeAttr("style");                    
                    }
                    break;
                case "compare":
                    if (controlVal != $("#" + min).val()) {
                        showError(controlid, "both value must be same.", errorDisplayType, texttype);
                        returnValues = false;
                    }
                    else {
                        $("#" + controlid).removeAttr("style");
                    }
                    break;
            }
            
            if (texttype == "alphanumeric" || texttype == "numeric" || texttype == "allcharacters" || texttype == "zipcode" || texttype == "mobile") {
                if (min == NaN || min == undefined)
                    min = 0;
                if (max == NaN || max == undefined)
                    max = 0;
                var minValue = parseFloat(min);
                var maxValue = parseFloat(max);
                var cValue = parseFloat(controlVal.length);

                if (texttype == "zipcode" || texttype == "mobile") {
                    if ((minValue == cValue && maxValue == cValue && $.isNumeric(controlVal)) || cValue == 0){
                        $("#" + controlid).removeAttr("style");
                    }
                    else {
                        showError(controlid, "Enter " + max + "characters.", errorDisplayType, texttype);
                        returnValues = false;
                    }
                }
                else {
                    if (minValue != 0) {
                        if (controlVal.length < min) {
                            showError(controlid, "Enter " + min + "characters.", errorDisplayType, texttype);
                            returnValues = false;
                        }
                        else {
                            $("#" + controlid).removeAttr("style");
                        }
                    }
                    if (maxValue != 0) {
                        if (controlVal.length > max) {
                            showError(controlid, "Enter max " + max + "characters.", errorDisplayType, texttype);
                            returnValues = false;
                        }
                        else {
                            $("#" + controlid).removeAttr("style");
                        }
                    }
                }
            }
        }
    }
    return returnValues;
}

function checkString(str, texttype) {
    var ok = false;
    for (var i = 0; i < str.length; i++) {
        if (texttype.indexOf(str[i]) == -1) {
            ok = true;
        }
    }
    return ok;
}

function restControl(id, texttype) {
    var element = $("#" + id);
    if (texttype == "select") {
        if (typeof element.prev().attr('style') !== typeof undefined) {
            element.prev().removeAttr("style");
        }
    }
    else {
        if (typeof element.attr('style') !== typeof undefined && element.attr('style') != false) {
            element.removeAttr("style");
        }
    }
}

function restControls(texttype) {
    $.each(listcontrols, function (key, value) {
        var element = $("#" + key);
        //$("#" + key).siblings().remove();
        if (texttype == "select") {
            if (typeof element.prev().attr('style') !== typeof undefined) {
                element.prev().removeAttr("style");
            }
        }
        else {
            if (typeof element.attr('style') !== typeof undefined && element.attr('style') != false) {
                element.removeAttr("style");
            }
        }
    });
}

function restAllControls(texttype) {
    
    var form = $('form');
    for (var i = 1; i < form[0].elements.length; i++) {
        var element = form[0].elements[i];
        if (texttype == "select")
        {
            if (typeof element.prev().attr('style') !== typeof undefined) {
                element.prev().removeAttr("style");
            }
        }
        else
        {
            if (typeof element.attr('style') !== typeof undefined && element.attr('style') != false) {
                element.removeAttr("style");
            }
        }
    }
}
function showError(id, error, mode, texttype) {
    try {
        var element = $("#" + id);
        //$("#" + id).siblings("kk").remove();
        if (texttype != "select") {            
            if (typeof element.attr('style') !== typeof undefined && element.attr('style') != false) {
                element.removeAttr("style");
            }
        }
        switch (mode) {
            case 1:
                //   $("#" + id).after("<kk><span style='color:red;'>" + error + "</span></kk>");
                if (texttype == "") {
                    element.prev().attr("style", "border:1px solid " + color);
                }
                else {
                    //element.attr("style", "border-color:" + color);
                    element.attr("style", "border:1px solid " + color);
                }
                break;
            case 2:
                element.after(imgWrong);
                break;
            case 3:
                element.after(" <kk><span style='font-weight:bold;color:" + color + ";'>" + error + "</span></kk><br />").after("<br />").after(imgWrong);
                break;
            default: element.after(imgWrong);
        }
    } catch (e) {
        //alert(e.Message);
    }
}

function showSuccess(controlid) {
    $("#" + controlid).siblings().remove();
    $("#" + controlid).after(imgCorrect);
}

function showMessage(msg) {
    $("#msg").text(msg);
}