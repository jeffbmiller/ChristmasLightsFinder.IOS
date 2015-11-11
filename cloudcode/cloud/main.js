// Use Parse.Cloud.define to define as many cloud functions as you want.
Parse.Cloud.define("likeHouse", function (request, response) {
    var House = Parse.Object.extend("House");
    var query = new Parse.Query(House);
    query.get(request.params.objectId, {
        success: function (house) {
            house.increment("likes");

            house.save(null, {
                success: function (object) {
                    response.success();
                },
                error: function (error) {
                    response.error(error);
                }
            });
        },
        error: function (object, error) {
            response.error("House liked failed");
        }
    });
});

Parse.Cloud.beforeSave("House", function (request, response) {

    var query = new Parse.Query("House");
    query.equalTo("address", request.object.get("address"));
    query.count({
        success: function (count) {
            console.log("Count " + count + " " + request.object.get("address"));
            if (count >= 1)
                response.error("Already a house with same address");
            else
                response.success();
        },
        error: function (error) {
            response.success();
        }
    });
});
