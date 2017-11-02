(function ($) {
    $.fn.extend({
        setImgModal: function (option) {
            var me = $(this);

            if ($(this).hasClass('modal-open')) {
                var opt = $.extend(true, {
                    uploadUrl: '',
                    cropUploadUrl: '',
                    targetImg: '',
                    imgAreaSelectSetting: {},
                    submitCallback: function (resultUrl) { },
                }, option);
            }

            me.data('option', opt);
            setImgModal(me);
        },

    });


///////////////////////////////////////////////////////////////////////////////////////////
    //設定Modal
    var setImgModal = function (btn) {
        var modal = btn.getModal();
        var opt = btn.getOption();
        /*
         檔案AJAX上傳
         */
        var fileupload = modal.find('.upload-file');

        fileupload.on('change', function (e) {
            var tempImg = modal.find('.modal-tempImg');
            var formdata = getFilesFormData($(this));
            formdata.append('ImageUrl', tempImg[0].src);
           
            fileUpload(formdata, opt.uploadUrl, function (result) {
                tempImg.attr('src', result + '?' + new Date().getTime());
                loadImgAreaSelectSetting(tempImg, opt.imgAreaSelectSetting);
            });
        });



        //設定開啟Modal時的點擊事件
        opt.OnOpen = function (e) {

            var modal = btn.getModal();
            var modalContent = modal.find('.modal-body');

            var tempImg = $('<img>');

            if (opt.targetImg.length) {
                tempImg = opt.targetImg.clone();
                tempImg[0].id = '';

            }

            tempImg.addClass('modal-tempImg');
            tempImg.appendTo(modalContent);
            loadImgAreaSelectSetting(tempImg, opt.imgAreaSelectSetting);

        }

        //設定提交Modal的函式
        opt.OnSubmit = function () {
            var targetImg = opt.targetImg;
            var tempImg = modal.find('.modal-tempImg');
            var fileupload = modal.find('.upload-file');

            var formdata = getFilesFormData(fileupload);

            if (tempImg.data('Section')) {
                formdata.append('Section', tempImg.data('Section'));
            }

            if (tempImg[0].src){
                formdata.append('ImageUrl', tempImg[0].src.split('?')[0]);
            }

            fileUpload(formdata, opt.cropUploadUrl, function (result) {

                targetImg[0].src = result;

                if (opt.submitCallback && typeof opt.submitCallback == 'function') {
                    opt.submitCallback(result);
                }
            });

            fileupload.val(null);
        }

        btn.setDefaultModal();

    };

    var fileUpload = function (formdata, Url, successFunc) {
        $.ajax({
            url: Url,
            type: 'post',
            processData: false,  // tell jQuery not to process the data 重要
            contentType: false,  // tell jQuery not to set contentType 重要
            data: formdata,
            dataType: 'json',
            success: function (result) {
                if (typeof successFunc == 'function') {
                    successFunc(result);
                }
            }

        });

    }

    var loadImgAreaSelectSetting = function (img, setting) {

        var defaultSetting = $.extend(true, setting, {
            parent: img.parent(),
            handles: true,
            hide: true,
            onSelectEnd: function (img, section) {
                section = $.extend(true, section, {
                    imgControlWidth: $(img).width(),
                    imgControlHeight: $(img).height(),

                });

                $(img).data('Section', JSON.stringify(section));
            
            }
        })
  
        img.imgAreaSelect(defaultSetting);

    }


    var getFilesFormData = function (fileInput, formData) {

        if (!formData) {
            formData = new FormData();
        }
        var files = fileInput[0].files;
        formData.append('Image', files[0]);

        return formData;
    };

})(jQuery);