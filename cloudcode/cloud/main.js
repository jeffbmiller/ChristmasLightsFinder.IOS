// Use Parse.Cloud.define to define as many cloud functions as you want.
var Image = require("parse-image");

Parse.Cloud.define("likeHouse", function (request, response) {
    var House = Parse.Object.extend("House");
    var query = new Parse.Query(House);
    query.get(request.params.objectId, {
        success: function (house) {
            house.increment("likes");
            house.save();
            response.success(house.get("likes"));
        },
        error: function (object, error) {
            response.error("House liked failed");
        }
    });
});

Parse.Cloud.beforeSave("House", function (request, response) {

    var query = new Parse.Query("House");
    query.equalTo("address", request.object.get("address"));
    query.notEqualTo("objectId", request.object.get("objectId"));
    query.count({
        success: function (count) {
            console.log("Count " + count + " " + request.object.get("address"));
            if (count >= 1)
                response.error("Already a house with same address");
            else {
                //response.success();
                //Create Thumbnail
                var house = request.object;
                if (house.get("image")) {
                    //Image is not modified so no need to create thumbnail
                    if (!house.dirty("image")) {
                        response.success();
                        return;
                    }
                    Parse.Cloud.httpRequest({
                        url: house.get("image").url()

                    }).then(function (response) {
                        var image = new Image();
                        return image.setData(response.buffer);

                    }).then(function (image) {
                        // Crop the image to the smaller of width or height.
                        var size = Math.min(image.width(), image.height());
                        return image.crop({
                            left: (image.width() - size) / 2,
                            top: (image.height() - size) / 2,
                            width: size,
                            height: size
                        });

                    }).then(function (image) {
                        // Resize the image to 64x64.
                        return image.scale({
                            width: 64,
                            height: 64
                        });

                    }).then(function (image) {
                        // Make sure it's a JPEG to save disk space and bandwidth.
                        return image.setFormat("JPEG");

                    }).then(function (image) {
                        // Get the image data in a Buffer.
                        return image.data();

                    }).then(function (buffer) {
                        // Save the image into a new file.
                        var base64 = buffer.toString("base64");
                        var cropped = new Parse.File("thumbnail.jpg", {base64: base64});
                        return cropped.save();

                    }).then(function (cropped) {
                        // Attach the image file to the original object.
                        house.set("thumbnail", cropped);

                    }).then(function (result) {
                        response.success();
                    }, function (error) {
                        response.error(error);
                    });
                }
                else {
                    response.success();
                }

            }

        },
        error: function (error) {
            response.success();
        }
    });
});

Parse.Cloud.afterSave("House", function (request) {

    if (request.object.existed() == false) {
        //Send Push Notificaiton
        Parse.Push.send({
            channels: ["Everyone"],
            data: {
                alert: "New House Added\n" + request.object.get("address") + "\n" + request.object.get("city") + ", " + request.object.get("province"),
                sound: "default"
            }
        }, {
            success: function () {
                // Push was successful
            },
            error: function (error) {
                console.error("Error sending new house added push notification");
            }
        });
    }
    else {
        return;
    }
});
