drop database if exists device_hub_yhlotest;
create database device_hub_yhlotest character set utf8mb4;
use device_hub_yhlotest;

create table receive_message (
    id bigint not null auto_increment comment '主键ID',
    instrument_id bigint not null comment '仪器ID',
    status tinyint not null comment '状态(0:待处理, 1:处理成功, 2:处理失败)',
    error_message varchar(500) not null comment '处理失败原因',
    create_time bigint not null comment '创建时间',
    primary key(id),
    key idx_instrument_id(instrument_id),
    key idx_status(status)
) engine=innodb default charset=utf8mb4 comment '接收仪器消息队列表';

create table receive_message_large (
    receive_message_id bigint not null comment '接收仪器消息队列表Id',
    raw_message longtext not null comment '原始报文',
    primary key(receive_message_id)
) engine=innodb default charset=utf8mb4 comment '接收仪器消息队列表扩展表';

create table receive_message_decode (
    id bigint not null auto_increment comment '主键ID',
    receive_message_id bigint not null comment '接收仪器消息队列表Id',
    type tinyint not null comment '类型(0:检验结果, 1:查询样本申请信息)',
    sample_no varchar(30) not null comment '样本号',
    barcode varchar(50) not null comment '条形码',
    result_json longtext not null comment '解码结果JSON',
    create_time bigint not null comment '创建时间',
    update_time bigint not null comment '更新时间',
    primary key(id),
    unique key uk_receive_message_id(receive_message_id)
) engine=innodb default charset=utf8mb4 comment '接收仪器消息解码结果表';

create table send_message (
    id bigint not null auto_increment comment '主键ID',
    instrument_id bigint not null comment '仪器ID',
    type tinyint not null comment '类型(0:申请单)',
    sample_no varchar(30) not null comment '样本号',
    barcode varchar(50) not null comment '条形码',
    status tinyint not null comment '状态(0:待处理，1:处理成功，2:处理失败)',
    create_time bigint not null comment '创建时间',
    update_time bigint not null comment '更新时间',
    primary key(id),
    key idx_status(status)
) engine=innodb default charset=utf8mb4 comment '发送仪器消息队列表';

create table send_message_large (
    send_message_id bigint not null comment '发送仪器消息队列表Id',
    send_json longtext not null comment '发送内容JSON',
    primary key(send_message_id)
) engine=innodb default charset=utf8mb4 comment '发送仪器消息队列表扩展表';

create table send_message_encoder (
    id bigint not null auto_increment comment '主键ID',
    send_message_id bigint not null comment '发送仪器消息队列表Id',
    send_content longtext not null comment '发送报文内容',
    create_time bigint not null comment '创建时间',
    update_time bigint not null comment '更新时间', 
    primary key(id),
    unique key uk_send_message_id(send_message_id)
) engine=innodb default charset=utf8mb4 comment '发送仪器消息编码记录';

create table client_log (
    id bigint not null auto_increment comment '主键ID',
    type tinyint not null comment '日志类型',
    level tinyint not null comment '状态(0:普通，1:警告，2:异常)',
    message longtext not null comment '日志内容',
    create_time bigint not null comment '创建时间',
    primary key(id)
) engine=innodb default charset=utf8mb4 comment '仪器通信日志表';