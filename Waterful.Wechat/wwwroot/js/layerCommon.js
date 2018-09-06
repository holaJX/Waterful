/**
 * Created by sl10 on 2017/6/6.
 */
// 提示框
function showLoyer(msg) {
    layer.open({
        content: msg,
//                style: 'background-color: #333333;font-size:16px; color:white; border:none;width:60%',
        time: 2, // 显示时间
        skin: 'msg',
//                shade: false // 取消遮罩
    });
}

// 确认框
function showConfirm(msg) {
    //询问框
    return new Promise(function(resolve, reject) {
        layer.open({
            content: msg,
            style: 'width:80%',
            btn: ['确认', '取消'],
            yes: function(index){
                layer.close(index);
                resolve(true);
            },
            no: function(index){
                layer.close(index);
                resolve(false);
            }
        })
    }).then(function(data) {
        return data;
    });
}
