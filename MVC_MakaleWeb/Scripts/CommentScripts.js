var mid = -1;
$(function () {  //sayfa yüklendiğinde
    $('#modal1').on('show.bs.modal', function (e) {
        var btn = $(e.relatedTarget);
         mid = btn.data("makaleid");
        //console.log(mid);  hatayı bulmak için

        $("#modal1_body").load("/Yorum/YorumGoster/"+mid);
    })
});
    function islemyap(btn, islem, yorumid, spanid)
    {
        var button = $(btn);
        var durum = button.data("editmod");
        if (islem == "update")
        {
            if (durum == false)
            {
                button.data("editmod", true);
                button.removeClass("btn-success");
                button.addClass("btn-danger");
                var span = button.find("span");//button içindeki spani yakala
                span.removeClass("glyphicon-edit"); //classını sil
                span.addClass("glyphicon-ok"); //clasına ekle
                $(spanid).attr("contenteditable", true);  /*false olan özelliğini tru yaptık*/
                $(spanid).focus();
            }

           


            else {
                button.data("editmod", false);
                button.removeClass("btn-danger");
                button.addClass("btn-success");
                var span = button.find("span");//button içindeki spani yakala
                span.addClass("glyphicon-edit"); //clasına ekle
                span.removeClass("glyphicon-ok"); //classını sil
                $(spanid).attr("contenteditable", false);  /*false olan özelliğini tru yaptık*/

                var yorum = $(spanid).text();
                $.ajax({
                    method: "POST",
                    url: "/Yorum/YorumGuncelle/" + yorumid,
                    data: { Text: yorum }
                }).done(function (sonuc) {
                    if (sonuc.hata) {
                        alert("yorum güncellenemedi.")

                    }
                    else {
                        $("#modal1_body").load("/Yorum/YorumGoster/" + mid);

                    }

                }).fail(function () {
                    alert("sunucu ile bağlantı kurulamadı");
                });              
            }        
        }
        else if (islem=="delete") {
            var onay = confirm("yorum silinsin mi?");
            if (!onay) {
                return false;
            }
            $.ajax({
                method: "GET",
                url: "/Yorum/YorumSil/" + yorumid
            }).done(function (sonuc) {
                if (sonuc.hata) {
                    alert("yorum silinemedi.");
                }
                else {
                    $("#modal1_body").load("/Yorum/YorumGoster/" + mid);
                }
            }).fail(function () { alert("sonuc ile bağlantı kurulamadı.") });
        }
        else if (islem == "insert") {
      
            var yorum = $("#yorum_text").val();
      
            $.ajax({
                method: "POST",
                url: "/Yorum/YorumEkle",
                data: { Test: yorum, id: mid }
            }).done(function (sonuc) {
                if (sonuc.hata) {
                    alert("yorum Eklenemedi.");
                }
                else {
                    $("#modal1_body").load("/Yorum/YorumGoster/" + mid);
                }
            }).fail(function () {
                alert("sunucu ile bağlantı kurulamadı")
            });
        }

    }

