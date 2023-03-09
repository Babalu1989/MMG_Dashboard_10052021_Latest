
function twodateComp(cdate1, cdate2) {
    var date1 = cdate1;
    var dd1 = date1.substring(0, 2);
    var mm1 = date1.substring(3, 5);
    var yy1 = date1.substring(6, date1.length);
    tmpdate1 = new Date(yy1, mm1 - 1, dd1);

    var date2 = cdate2;
    var dd2 = date2.substring(0, 2);
    var mm2 = date2.substring(3, 5);
    var yy2 = date2.substring(6, date2.length);
    tmpdate2 = new Date(yy2, mm2 - 1, dd2);

    if (tmpdate1 > tmpdate2)
        return 1;
    else if (tmpdate1 == tmpdate2)
        return 0;
    else
        return -1;
}

function twodateComp1(cdate1, cdate2) {
    var date1 = cdate1;
    var dd1 = date1.substring(0, 2);
    var mm1 = date1.substring(3, 5);
    var yy1 = date1.substring(6, date1.length);
    tmpdate1 = new Date(yy1, mm1 - 1, dd1);

    var date2 = cdate2;
    var dd2 = date2.substring(0, 2);
    var mm2 = date2.substring(3, 5);
    var yy2 = date2.substring(6, date2.length);
    tmpdate2 = new Date(yy2, mm2 - 1, dd2);

    if (tmpdate1 > tmpdate2) {
        return 1;
    }
    else if (tmpdate1 < tmpdate2) {
        return -1;
    }
    else {
        return 0;
    }
}


function isNumber(obj, displayname) {
    var numvalue;
    numvalue = new String();
    numvalue = obj.value;

    if (isNaN(obj.value) || numvalue.indexOf("e") != -1) {
        alert(displayname + " should be numeric");
        obj.value = '';
        obj.focus();
        return false;
    }
    return true;
}

function isNumeric(obj, displayname) {
    var numvalue;
    numvalue = new String();
    numvalue = obj.value;

    if (isNaN(obj.value) || numvalue.indexOf("e") != -1) {
        alert(displayname + " should be numeric");
        obj.focus();
        return false;
    }
    return true;
}


function isNonNumber(obj, displayName) {
    var str = lTrim(obj.value);
    var flag = false;
    if (str.length > 0) {
        for (i = 0; i < str.length; i++) {
            if ((str.charAt(0)) == "-") {
                i = i + 1;
                continue;
            }
            if (str.charAt(i) < '0' || str.charAt(i) > '9') {
                flag = true;
            }
        }
        if (flag)
            return true;
        else {
            alert(displayName + "should be non-numeric.");
            obj.focus();
            return false;
        }
    }
    else
        return true;
    return flag;
}




function isCheckValidDate(obj, dt) {
    newdt = new Date(dt);
    str = (newdt.getMonth() + 1) + "/" + newdt.getDate() + "/" + newdt.getFullYear();
    if (dt == str) return true;
    alert("Invalid date.");
    obj.focus();
    return false;
}


function isInt(theStr) {
    var flag = true;

    if (isEmpty(theStr)) { flag = false; }
    else
    {
        for (var i = 0; i < theStr.length; i++) {
            if (isDigit(theStr.substring(i, i + 1)) == false) {
                flag = false; break;
            }
        }
    }
    return (flag);
}



function validateFloatKeyPress(el, evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    var number = el.value.split('.');
    if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
        alert('Only numbers allow');
        return false;
    }
    //just one dot
    if (number.length > 1 && charCode == 46) {
        return false;
    }
    //get the carat position
    var caratPos = getSelectionStart(el);
    var dotPos = el.value.indexOf(".");
    if (caratPos > dotPos && dotPos > -1 && (number[1].length > 1)) {
        return false;
    }
    return true;
}


function getSelectionStart(o) {
    if (o.createTextRange) {
        var r = document.selection.createRange().duplicate()
        r.moveEnd('character', o.value.length)
        if (r.text == '') return o.value.length
        return o.value.lastIndexOf(r.text)
    } else return o.selectionStart
}

