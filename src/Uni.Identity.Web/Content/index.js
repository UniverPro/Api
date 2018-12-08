/// <reference path="../node_modules/@types/jquery/index.d.ts" />
// ReSharper disable Es6Feature
"use strict";
import "jquery";
import "jquery-validation/dist/jquery.validate";
import "jquery-validation/dist/additional-methods";
import "jquery-validation/dist/localization/messages_ru";
import "jquery-validation-unobtrusive/dist/jquery.validate.unobtrusive";
import "bootstrap";
import "./styles/index.scss";
import "./js/index";

$(function() {
    // Enable tooltips everywhere
    $("[data-toggle=\"tooltip\"]").tooltip();
    // Enable popovers everywhere
    $("[data-toggle=\"popover\"]").popover();
});
// ReSharper restore Es6Feature