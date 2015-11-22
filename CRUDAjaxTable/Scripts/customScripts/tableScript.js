(function () {
    var getAll = "api/operations/";


    function drawRow(jsonArray, counter) {
        var row = $("<tr/>");
        $("#table").append(row);
        row.append($("<td>" + counter + "</td>"));
        row.append($("<td>" + jsonArray.author.name + "</td>"));
        row.append($("<td>" + jsonArray.cost + "</td>"));
        row.append($("<td>" + jsonArray.typeOperation + "</td>"));
        row.append($("<td>" + jsonArray.description + "</td>"));
        row.append($("<td>" + '<a class=\"itemEditSelector\"' +
         ' data-reveal-id="editPopUp" data-animation="fadeAndPop" ' +
         'data-animationspeed="300" data-closeonbackgroundclick="true" ' +
        'data-dismissmodalclass="close-reveal-modal" id=\"' +
        jsonArray.id + '\" href=\"#"\">Edit</a>' + "</td>"));
        row.append($("<td>" + '<a class=\"itemDeleteSelector\"' +
            ' data-reveal-id="deletePopUp" data-animation="fadeAndPop" ' +
            'data-animationspeed="300" data-closeonbackgroundclick="true" ' +
            'data-dismissmodalclass="close-reveal-modal" id=\"' +
            jsonArray.id + '\" href=\"#"\">Delete</a>' + "</td>"));
    }

    function drawTable(jsonArray) {
        for (var i = 0; i < jsonArray.length; i++) {
            var counter = i + 1;
            drawRow(jsonArray[i], counter);
        }
    }

    function fillTable(url) {
        $.ajax(
        {
            url: url,
            success: function (jsonArray) {
                drawTable(jsonArray);

                $(".itemDeleteSelector").click(function (event) {
                    deleteItem(getAll + event.target.id);
                });
                $(".itemEditSelector").click(function (event) {
                    editItem(getAll + event.target.id);
                });
              
            }
        });
    }
  
    function editItem(url) {
        $.ajax(
        {
            url: url,
            success: function (itemInfo) {
                // select popup
                var popUp = $("#editPopUp");
                // remove all items that been before
                popUp.find('*').not('.close-reveal-modal').remove();
                popUp.append("<label>Author Name</label><br\>");

                popUp.append('<input type=\"text\" id="authorName" name=\"authorName\" value=\"'
                    + itemInfo.author.name + '\"><br\>');
                popUp.append("<label>Cost</label><br\>");
                popUp.append('<input type=\"number\" id="cost" name=\"cost\" value=\"'
                    + itemInfo.cost + '\"><br\>');
                popUp.append("<label>TypeOfOperation</label><br\>");
                popUp.append("<select id=\"mySelect\"></select><br\>");
                // get all items from json to option
                $.each(itemInfo.allOperations, function (index, item) {
                    $("#mySelect").append(
                        $("<option></option>")
                            .text(item)
                            .val(item)
                    );
                });
                $('select option:[value=\"' + itemInfo.typeOperation + '\"]').attr('selected', true);
                popUp.append("<label>Description</label><br\>");
                popUp.append('<input type=\"text\" id="description" name=\"description\" value=\"'
                    + itemInfo.description + '\"><br\><br\>');
                // put in id selected primary key to get value from db
                popUp.append('<a href=\"#\" class=\"btn btn-primary\" id=\"' + itemInfo.id + '\" >Edit</a>');


                $(".btn.btn-primary").click(function (event) {
                    var items = {}
                    // insided json
                    items.author = {}
                    items.author.name = $("#authorName").val();
                    items.id = event.target.id;
                    items.description = $("#description").val();
                    items.cost = $("#cost").val();
                    items.typeOperation = $("#mySelect").val();
                    $.ajax({

                        url: getAll,
                        type: "PUT",
                        dataType: "json",
                        data: JSON.stringify(items),
                        contentType: "application/json",
                        success: function () {

                            var table = $("#table");
                            table.fadeOut(100);
                            // clear all table but not header 
                            $('table tr:not(:first)').remove();
                            fillTable(getAll);
                            table.fadeIn(500);
                            popUp.trigger('reveal:close');
                            location.reload();
                        }
                    });
                });
            }
        });
    }
    function deleteItem(url) {
        $.ajax(
        {
            url: url,
            success: function (itemInfo) {
                var popUp = $("#deletePopUp");
                popUp.find('*').not('.close-reveal-modal').remove();
                popUp.append("<h3>Are you sure you want to delete?</h3>");
                popUp.append("<p>" + itemInfo.author.name + "</p>");
                popUp.append("<p>" + itemInfo.cost + "</p>");
                popUp.append("<p>" + itemInfo.typeOperation + "</p>");
                popUp.append("<p>" + itemInfo.description + "</p>");

                popUp.append('<a href=\"#\" class=\"btn btn-danger\" id=\"' + itemInfo.id + '\" >Delete</a>');
                $(".btn.btn-danger").click(function (event) {
                    $.ajax({

                        url: getAll + event.target.id,
                        type: "DELETE",

                        success: function () {

                            var table = $("#table");
                            table.fadeOut(100);
                            $('table tr:not(:first)').remove();
                            fillTable(getAll);
                            table.fadeIn(500);
                            popUp.trigger('reveal:close');
                        }
                    });
                });
            }
        });
    }

    fillTable(getAll);
    $(document).ready(function() {
        $("#addElement").click(function(event) {
            addItem();
           
        });
    });
    function addItem() {
        // select popup
        var popUp = $("#addPopUp");
        // remove all items that been before
        popUp.find('*').not('.close-reveal-modal').remove();
        popUp.append("<label>Author Name</label><br\>");
        popUp.append('<input type=\"text\" id="authorName" name=\"authorName\"><br\>');
        popUp.append("<label>Cost</label><br\>");
        popUp.append('<input type=\"number\" id="cost" name=\"cost\" <br\><br\>');
        popUp.append("<label>TypeOfOperation</label><br\>");
        popUp.append("<select id=\"mySelect\"></select><br\>");
        // get all items from json to option
        $('#mySelect').append($('<option>', {
            value: 'Income',
            text: 'Income'
        }));
        $('#mySelect').append($('<option>', {
            value: 'OutCome',
            text: 'OutCome'
        }));
        popUp.append("<label>Description</label><br\>");
        popUp.append('<input type=\"text\" id="description" name=\"description\" value=""><br\><br\>');
        // put in id selected primary key to get value from db
        popUp.append('<a href=\"#\" class=\"btn btn-primary\" id=\"add\">Add</a>');

        $("#add").click(function (event) {
            var items = {}
            // insided json
            items.author = {}
            items.author.name = $("#authorName").val();
            items.description = $("#description").val();
            items.cost = $("#cost").val();
            items.typeOperation = $("#mySelect").val();
            $.ajax({

                url: getAll,
                type: "POST",
                dataType: "json",
                data: JSON.stringify(items),
                contentType: "application/json",
                success: function () {

                    var table = $("#table");
                    table.fadeOut(100);
                    // clear all table but not header 
                    $('table tr:not(:first)').remove();
                    fillTable(getAll);
                    table.fadeIn(500);
                    popUp.trigger('reveal:close');
                    location.reload();
                }
            });
        });
    }


})();