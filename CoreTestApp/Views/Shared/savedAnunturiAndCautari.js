//Globally accessible(can remove var to be globally accessible and remove from here, but to be more readable...)
//Use timestamp for testing with browser caching <script src="@Url.Content("~/Views/Shared/savedAnunturiAndCautari.js?<"+@DateTime.Now.Ticks.ToString()+">")"></script>
var anunturiSalvateList;
var cautariSalvateList;
$(function() {
    //alert("bau3");
    anunturiSalvateList = new CookieList("AnunturiSalvate");
    cautariSalvateList = new CookieList("CautariSalvate");

    if (anunturiSalvateList.items().length > 0) {
        $("#footer").show();
        $("#anunturiSalvateHeaderImage").toggleClass('red');
        $(".anunutiSalvateNumber").text("Anunturi Salvate " + anunturiSalvateList.items().length);
        $("#anunturiSalvateHeaderText").text(anunturiSalvateList.items().length + " Salvate");
    }

    if (cautariSalvateList.items().length > 0) {
        $("#cautariSalvateHeaderImage").addClass('red');
        $("#salvareCautareStar").addClass('red');
        $("#cautariSalvateHeaderText").text(cautariSalvateList.items().length + "Căutări");
    }

    $(".salveazaAnunt").unbind('click').on("click", function() {
        if (anunturiSalvateList.items().length == 20) {
            alert("Limita maximă de anunțuri salvate(20) atinsă! Vă rugăm ștergeți unele dintre anunțurile salvate pentru a salva altele noi.");
            return;
        }
        var value = $(this).closest(".descriereApartament").find(".imobilIdClass").text();
        if (($(this).find("span").hasClass('red'))) {
            $(this).find("p").text("Salvează");
            anunturiSalvateList.remove(value);
        } else {
            $(this).find("p").text("Salvat");
            anunturiSalvateList.add(value);
            $("#anunturiSalvateHeaderImage").addClass('red');
        }
        $(".anunutiSalvateNumber").text("Anunturi Salvate " + anunturiSalvateList.items().length);
        $("#anunturiSalvateHeaderText").text(anunturiSalvateList.items().length + " Salvate");

        $(this).find("span").toggleClass('red');
        $("#footer").show();
        $("#footer").stop(true);
        $("#footer").effect("pulsate", null, 100, null);
        $(".anunutiSalvateHeader").effect("pulsate", null, 100, null);
    });

    $("#SalveazaCautareButton").unbind('click').on("click", function() {
        if (cautariSalvateList.items().length == 5) {
            alert("Limita maximă de căutari salvate(5) atinsă! Vă rugăm ștergeți unele dintre căutările salvate pentru a salva altele noi.");
            return;
        }
        var parameterStartIndex = window.location.href.indexOf("judet-");
        cautariSalvateList.add(window.location.href.substring(parameterStartIndex, window.location.href.length));
        updateSavedCautari();

        $("#cautariSalvateHeaderText").text(cautariSalvateList.items().length + "Căutări");
        $("#cautariSalvateHeaderImage").addClass('red');
        $("#salvareCautareStar").addClass('red');
        $("#cautariHeaderImage").effect("pulsate", null, 100, null);
    });

    $(".anunutiSalvateNumber").unbind('click').on("click", updateSavedAnunturi);
    $(".anunutiSalvateHeader").unbind('click').on("click", updateSavedAnunturi);
    $("#cautariHeaderImage").unbind('click').on("click", updateSavedCautari);

    $("#fadeLayout").unbind('click').on("click", function() {
        $("#anunturiSalvate").hide();
        $("#fadeLayout").hide();
    });

    $(".selectBoxSimulator li").click(function() {
        $(this).parents('.selectBoxSimulator').find('button').html($(this).text() + "<span class=\"caret\"></span>");
        $(this).parents('.selectBoxSimulator').find('button').css("font-weight", "bold");
        $(this).parents('.selectBoxSimulator').find('input').val($(this).data("listvalue"));
        $(this).parents('.selectBoxSimulator').find('li').removeClass("selBoxSelected");
        $(this).addClass("selBoxSelected");
    });
});

function updateSavedCautari() {
    document.getElementById('anunturiSalvate').style.display = 'block';
    document.getElementById('fadeLayout').style.display = 'block';
    $("#anunturiSalvate").empty();
    if (cautariSalvateList.items().length > 0) {
        $("#anunturiSalvate").append("<div style=\"cursor: pointer; margin-bottom: 7px\" onclick=\"document.getElementById('anunturiSalvate').style.display='none'; document.getElementById('fadeLayout').style.display='none'; \"><div class = \"btn btn-info\"><span class=\"glyphicon glyphicon glyphicon-off\"></span><p style=\"display: inline-block; margin: 3px\">Inchide</p></div></div>" +
            "<button style=\"width:215px; margin-bottom:16px\" class=\"btn btn-danger\" onclick=\" clearAllSavedCautari() \"><span class=\"glyphicon glyphicon-trash\"></span> Sterge Cautarile Salvate</button>");
    } else {
        $("#anunturiSalvate").append("<div style=\"cursor: pointer; margin-bottom: 7px\" onclick=\"document.getElementById('anunturiSalvate').style.display='none'; document.getElementById('fadeLayout').style.display='none'; \"><div class = \"btn btn-info\"><span class=\"glyphicon glyphicon glyphicon-off\"></span><p style=\"display: inline-block; margin: 3px\">Inchide</p></div></div>" +
            "<h3>Nu aveți căutări salvate</h3><br/><h3>Pentru a salva o căutare, apăsați butonul Salvează căutarea (vezi imagine)</h3><img src=\"/Images/exempluSalvareCautare.png\" style=\"width:150px\" />");
    }
    for (var i = 0; i < cautariSalvateList.items().length; i++) {
        if (cautariSalvateList.items()[i] === document.URL.substring(document.URL.indexOf("judet-"), document.URL.length)) {
            $("#anunturiSalvate").append("<div class=\"alert alert-info\" style=\"cursor: pointer; margin-bottom: 0px; padding-left: 2px\"><h3 style=\"margin-top: 0px\">" + (i + 1) + ". " + cautariSalvateList.items()[i] + "</h3></div>");
        } else {
            //alert(document.URL.substring(0, document.URL.indexOf("judet-")));
            $("#anunturiSalvate").append("<div style=\"cursor: pointer;\" onclick=\"window.location.href = '" + "http://" + location.host + "/" + cautariSalvateList.items()[i] + "';\"><h3>" + (i + 1) + ". " + cautariSalvateList.items()[i] + "</h3></div><br/>");
        }
    }
}

function updateSavedAnunturi() {
    document.getElementById('anunturiSalvate').style.display = 'block';
    document.getElementById('fadeLayout').style.display = 'block';
    $("#anunturiSalvate").empty();
    $("#anunturiSalvate").append("<img src=\"/Images/preloader.gif\" width=\"40px\"\>");
    var postData = { savedIds: anunturiSalvateList.items() };
    $.ajax({
        type: "POST",
        url: "/Home/GetSavedAnunturiFromCookie",
        data: postData,
        success: function(data) {
            $("#anunturiSalvate").empty();
            if (data.length > 0) {
                $("#anunturiSalvate").append("<div style=\"cursor: pointer; margin-bottom: 7px\" onclick=\"document.getElementById('anunturiSalvate').style.display='none'; document.getElementById('fadeLayout').style.display='none'; \"><div class = \"btn btn-info\"><span class=\"glyphicon glyphicon glyphicon-off\"></span><p style=\"display: inline-block; margin: 3px\">Inchide</p></div></div>" +
                    "<button style=\"margin-bottom:16px\" class=\"btn btn-danger\" onclick=\" clearAllSavedAnunturi() \"><span class=\"glyphicon glyphicon-trash\"></span> Sterge Anunturile Salvate</button>");
            } else {
                $("#anunturiSalvate").append("<div style=\"cursor: pointer; margin-bottom: 7px\" onclick=\"document.getElementById('anunturiSalvate').style.display='none'; document.getElementById('fadeLayout').style.display='none'; \"><div class = \"btn btn-info\"><span class=\"glyphicon glyphicon glyphicon-off\"></span><p style=\"display: inline-block; margin: 3px\">Inchide</p></div></div>" +
                    "<h3>Nu aveți anunțuri salvate</h3><br/><h3>Pentru a salva un anunț, apăsați butonul Salvează (vezi imagine)</h3><img src=\"/Images/exempluSalvareAnunt.png\" style=\"width:150px\" />");
            }
            for (var i = 0; i < data.length; i++) {
                var split = data[i].split('|');
                $("#anunturiSalvate").append("<div style=\"height:100px; width:400px; border-bottom: 1px solid #a7d582\" onmouseover=\"this.style.background='#F9F9F9';\" onmouseout=\"this.style.background='white';\">" +
                        "<div style=\"display:inline-block; width:110px; vertical-align:top; padding-top:5px;\">" +
                        "<img height=\"80px\" style=\"cursor:pointer\" alt=\"Anunt imobiliar Salvat\" src=\"/Images/" + split[4] + "\" onclick=\" window.location = 'http://" + location.host + "/Home/ApartamentDetalii?imobilId=" + split[0] + "';\"/>" +
                        "</div>" +
                        "<div style=\"display:inline-block; width: 210px; white-space: nowrap; overflow: hidden;\">" +
                        "<p style=\"margin: 5px 0 2px 5px\">ID: " + split[0] + " Titlu: " + split[1] + "</p>" +
                        "<p style=\"margin: 5px 0 2px 5px\">" + split[2] + ", " + split[3] + "</p>" +
                        "<button class=\"ViewAnuntSalvatButton btn btn-success\" onclick=\" window.location = 'http://" + location.host + "/Home/ApartamentDetalii?imobilId=" + split[0] + "';\">Arata</button>" +
                        "<button class=\"EliminaAnuntSalvatButton btn btn-danger\" onclick=\"stergeAnuntSalvat(" + split[0] + ")\"><img src=\"/Images/close_black.png\" \></button>" +
                        "</div>" +
                        "</div>");
            }
        },
        error: function() {
            $("#anunturiSalvate").empty();
            $("#anunturiSalvate").append("<div style=\"cursor: pointer; margin-bottom: 7px\" onclick=\"document.getElementById('anunturiSalvate').style.display='none'; document.getElementById('fadeLayout').style.display='none'; \"><div class = \"btn btn-info\"><span class=\"glyphicon glyphicon glyphicon-off\"></span><h3 style=\"display: inline-block; margin: 3px\">Inchide</h3></div></div>" +
                "<h3><span class=\"glyphicon glyphicon-info-sign \"></span>A aparut o eroare la incarcarea anunturilor salvate</h3>");
        },
        dataType: "json",
        traditional: true
    });
}

function clearAllSavedAnunturi() {
    anunturiSalvateList.clear();
    $(".anunutiSalvateNumber").text("Anunturi Salvate " + anunturiSalvateList.items().length);
    $("#anunturiSalvateHeaderText").text("Salvate");
    $("#anunturiSalvateHeaderImage").removeClass('red');
    $(".salveazaAnunt").find("span").removeClass('red');
    $(".salveazaAnunt").find("p").text("Salveaza");
    $("#footer").hide();
    updateSavedAnunturi();
}

function clearAllSavedCautari() {
    cautariSalvateList.clear();
    $("#cautariSalvateHeaderImage").removeClass('red');
    $("#salvareCautareStar").removeClass('red');
    $("#cautariSalvateHeaderText").text("Căutări");
    updateSavedCautari();
}

function stergeAnuntSalvat(id) {
    anunturiSalvateList.remove(id.toString());
    $(".anunutiSalvateNumber").text("Anunturi Salvate " + anunturiSalvateList.items().length);
    $("#anunturiSalvateHeaderText").text(anunturiSalvateList.items().length + " Salvate");

    //Search if on page to deselect
    var saved = $(".imobilIdClass").filter(function() {
        return $(this).text() == id;
    });
    if (saved.length > 0) {
        saved.parent().find(".salveazaAnunt p").text('Salvează');
        saved.parent().find(".salveazaAnunt span").toggleClass('red');
    }
    updateSavedAnunturi();
}

//http://stackoverflow.com/questions/1959455/how-to-store-an-array-in-jquery-cookie
//http://www.jibbering.com/faq/faq_notes/closures.html
var CookieList = function(cookieName) {
    //When the cookie is saved the items will be a comma seperated string
    var cookie = $.cookie(cookieName);
    var items = cookie ? cookie.split(/,/) : new Array();

    return {
        "add": function(val) {
            //remove null after list is initialised
            var nullIndex = items.indexOf("null");
            if (nullIndex != -1) {
                items.splice(nullIndex, 1);
            }
            //do not add duplicates
            if (items.indexOf(val) == -1) {
                items.push(val);
                $.cookie(cookieName, items.join(','), { expires: 7, path: '/' });
            }
        },
        "remove": function(val) {
            var indx = items.indexOf(val);
            if (indx != -1) items.splice(indx, 1);
            $.cookie(cookieName, items.join(','), { expires: 7, path: '/' });
        },
        "clear": function() {
            items = new Array();
            $.cookie(cookieName, items.join(','), { expires: 7, path: '/' });
        },
        "items": function() {
            //remove null after list is initialised
            var nullIndex = items.indexOf("null");
            if (nullIndex != -1) {
                items.splice(nullIndex, 1);
            }
            return items;
        }
    };
};