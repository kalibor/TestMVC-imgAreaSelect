(function ($) {

    $.fn.extend({
        setDefaultModal: function () {
            var me = $(this);

            if (me.hasClass('modal-open')) {
                var modal = getModal(me);
                var opt = getOption(me);
                if (modal.length) {

                    var span = modal.find('.close');

                    me.on('click', function () {
                        openModal(modal, opt.OnOpen)

                    });

                    span.on('click', function () {

                        closeModal(modal);
                    });

                    $(window).on('click', function (event) {
                            if (event.target == modal[0]) {
                                closeModal(modal);
                            }
                      
                    });

                

                }

            }

        },

        submit: function (modal,func) {
            var me = $(this);
            if (me.hasClass('modal-submit')) {
                if (func && typeof func == 'function') {
                    func();
                    closeModal(modal);
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

    var openModal = function (modal,func) {
        modal.css('display', "block");
        if (func && typeof func == 'function') {
            func();
        }

    }

})(jQuery);


