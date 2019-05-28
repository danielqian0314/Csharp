(function ($) {

    var ItemUnitDict = {};

    var targetJq = undefined;

    var SelectedUnit = [];


    var onMultiSelectChange = function (option, checked, select) {

        var uint_p = ItemUnitDict[$(option).val()];

        var targetItem = _.find(SelectedUnit, { unit: uint_p });
        if (_.isUndefined(targetItem)) {
            SelectedUnit.push({ "unit": uint_p, "count": 1 });

            if (SelectedUnit.length > 2) {
                console.log("lock");
                Lock();
            }

        }
        else {

            if (checked) {
                targetItem.count++;
            }
            else {
                targetItem.count--;
                if (targetItem.count == 0) {
                    SelectedUnit = _.without(SelectedUnit, targetItem);
                }

                if (SelectedUnit.length <= 2) {
                    console.log("unlock");
                    Unlock();
                }
            }
        }

        console.log(SelectedUnit);
    };

    var Lock = function ()
    {
        console.log("XXX lock");
        var selectedOptions = targetJq.children('option:selected');
        console.log(selectedOptions);
        var nonSelectedOptions = targetJq.children('option').filter(function () {

            var targetItem = _.find(SelectedUnit, { unit: $(this).data("unit") });
            var isUndefined = _.isUndefined(targetItem);

            return !$(this).is(':selected') && isUndefined;
        });

        var dropdown = targetJq.siblings('.multiselect-container');
        nonSelectedOptions.each(function () {
            var input = $('input[value="' + $(this).val() + '"]');
            input.prop('disabled', true);
            input.parent('li').addClass('disabled');
        });
    }

    var Unlock = function () {
        var dropdown = targetJq.siblings('.multiselect-container');
        targetJq.children('option').each(function () {
            var input = $('input[value="' + $(this).val() + '"]');
            input.prop('disabled', false);
            input.parent('li').addClass('disabled');
        });
    };


    $.fn.multiselect_history_page = function (options) {

        targetJq = this;
        var $this = $(this);

        var settings = $.extend({
            disableIfEmpty: true,
            buttonWidth: '438px',
            maxHeight: 400,
            checkboxName: 'multiselect[]',
            nonSelectedText: '[EN]Check an option!',
            nSelectedText: '[EN]- Too many options selected!',
            allSelectedText: '[EN] No option left ...',
            numberDisplayed: 3,
            onChange: onMultiSelectChange
        }, options);

        this.children("option").each(function (i, v) {
            ItemUnitDict[$(this).val()] = $(this).data("unit");
        });


        this.children("option:selected").each(function (i, v) {
            var uint_p = $(this).data("unit");
            var targetItem = _.find(SelectedUnit, { unit: uint_p });


            if (_.isUndefined(targetItem)) {
                SelectedUnit.push({ "unit": uint_p, "count": 1 });
            }
            else {
                targetItem.count++;
            }

        });

        this.multiselect(settings);

        if (SelectedUnit.length >= settings.numberDisplayed) {
            console.log(SelectedUnit);

            Lock();
        }

        return this;
    };
})(jQuery);