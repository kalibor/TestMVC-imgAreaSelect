(function ($) {

    $.fn.extend({
        setDefaultModal: function () {
            var me = $(this);

            if (me.hasClass('modal-open')) {
                var modal = getModal(me);
                var opt = getOption(me);
                if (modal.length) {
                    //1
                    me.on('click', function () {
                        openModal(modal, opt.OnOpen)
                    });

                    //2
                    var span = modal.find('.close');
                    span.on('click', function () {

                        closeModal(modal);
                    });

                    //3
                    $(window).on('click', function (event) {
                        if (event.target == modal[0]) {
                            closeModal(modal);
                        }
                    });

                    //4
                    var btnSubmit = modal.find('.modal-submit');
                    btnSubmit.on('click', function () {
                        submitModal(modal, opt.OnSubmit)
                    });

                }

            }

        },

        getModal: function () {
            var me = $(this);

            if (me.hasClass('modal-open')) {
                return getModal(me);
            } else {
                return null;
            }

        },
        getOption: function () {
            var me = $(this);
            if (me.hasClass('modal-open')) {
                return getOption(me);
            } else {
                return null;
            }
        }

    });

    ////////////////////////////////////////////////////////////////////


    var getModal = function (btn) {
        var targetModalId = btn.data('target');

        return $(document.getElementById(targetModalId));

    };


    var getOption = function (btn) {
        var opt = btn.data('option');
        return opt;

    };

    var closeModal = function (modal) {

        var modalContent = modal.find('.modal-body');
        modalContent.empty();
        modal.css('display', "none");

    };

    var openModal = function (modal, func) {
        modal.css('display', "block");
        if (func && typeof func == 'function') {
            func();
        }

    }

    var submitModal = function (modal, func) {
     
        if (func && typeof func == 'function') {
            func();
        }

        closeModal(modal);
    }

})(jQuery);


