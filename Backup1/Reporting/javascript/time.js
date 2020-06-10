//
/// Global time variables
//
var curDate;
var curHour = 1;
var curMin = 0;
var curAmPm = 1;

// Creates the time values required to fill in the Date/Time
// choosers on each of the reports. Stores the results in the
// following global variables: curHour, curMin, curAmPm
function findTime() {

    var now = new Date();
    curDate = formatDate(now);
    curHour = now.getHours();
    if(curHour > 12) {

        curAmPm = 2;
        curHour = (curHour-12);
    } else {
        curAmPm = 1;
    }
    curMin = 0;
}

// Formats a date how I want to see it, so there!
function formatDate(now) {

    var formated = "" + now.getDate() + "-";
    switch(now.getMonth()) {
    case 0:
        formated += "Jan";
        break;
    case 1:
        formated += "Feb";
        break;
    case 2:
        formated += "Mar";
        break;
    case 3:
        formated += "Apr";
        break;
    case 4:
        formated += "May";
        break;
    case 5:
        formated += "Jun";
        break;
    case 6:
        formated += "Jul";
        break;
    case 7:
        formated += "Aug";
        break;
    case 8:
        formated += "Sep";
        break;
    case 9:
        formated += "Oct";
        break;
    case 10:
        formated += "Nov";
        break;
    case 11:
        formated += "Dec";
        break;
    default:
        formated += "Jan";
    }
    formated += ("-" + now.getFullYear());
    return formated;
}

// Creates a setion of 'select' options for choosing
// the current hour in 12-hour format.
function create12HourList() {
    
    html = "";
    html += "<option value=\"1\">1</option>\n";
    html += "<option value=\"2\">2</option>\n";
    html += "<option value=\"3\">3</option>\n";
    html += "<option value=\"4\">4</option>\n";
    html += "<option value=\"5\">5</option>\n";
    html += "<option value=\"6\">6</option>\n";
    html += "<option value=\"7\">7</option>\n";
    html += "<option value=\"8\">8</option>\n";
    html += "<option value=\"9\">9</option>\n";
    html += "<option value=\"10\">10</option>\n";
    html += "<option value=\"11\">11</option>\n";
    html += "<option value=\"12\">12</option>\n";
    return html;
}

// Create a set of 'select' options for choosing
// time in 5 minute increments.
function create5MinList() {

    html = "";
    html += "<option value=\"00\">00</option>\n";
    html += "<option value=\"05\">05</option>\n";
    html += "<option value=\"10\">10</option>\n";
    html += "<option value=\"15\">15</option>\n";
    html += "<option value=\"20\">20</option>\n";
    html += "<option value=\"25\">25</option>\n";
    html += "<option value=\"30\">30</option>\n";
    html += "<option value=\"35\">35</option>\n";
    html += "<option value=\"40\">40</option>\n";
    html += "<option value=\"45\">45</option>\n";
    html += "<option value=\"50\">50</option>\n";
    html += "<option value=\"55\">55</option>\n";
    html += "<option value=\"59\">59</option>\n";
    return html;
}
