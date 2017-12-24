var packagesController = function (dataService) {
    var obj = {};

    function enable(id) {
        dataService.put('blogifier/api/packages/enable/' + id, obj, done, fail);
    }

    function disable(id) {
        dataService.put('blogifier/api/packages/disable/' + id, obj, done, fail);
    }

    function done(data) {
        toastr.success('Updated');
        setTimeout(function () {
            window.location.href = getUrl('admin/packages/widgets');
        }, 2000);
    }

    function fail() {
        $('.loading').fadeOut();
        toastr.error('Failed');
    }

    function showInfo(id) {
        dataService.get('blogifier/api/packages/' + id, infoCallback, fail);
        return false;
    }

    function infoCallback(data) {
        $('#packageInfoLabel').html(data.title);

        $('.bf-package-info-cover').find("img").attr('src', data.cover);
        $('.bf-package-info-logo').attr('src', data.icon);
        $('.bf-package-info-title').html(data.title);
        $('.bf-package-info-desc').html(data.description);

        $('.bf-package-info-version').html(data.version);
        $('.bf-package-info-date').html(getDate(data.lastUpdated));
        $('.bf-package-info-developer').html(data.author);

        $('#packageInfo').modal();
    }

    function addWidgetToZone(btn, zone) {
        var widget = $(btn).parent().parent().find('select').val();
        toastr.success(zone + ' :: ' + widget);
    }

    return {
        enable: enable,
        disable: disable,
        showInfo: showInfo,
        addWidgetToZone: addWidgetToZone
    }
}(DataService);