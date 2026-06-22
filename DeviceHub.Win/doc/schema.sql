/*create table instrument (
    id bigint not null auto_increment comment '主键ID',
    instrument_code varchar(50) not null comment '仪器编号',
    instrument_name varchar(100) not null comment '仪器名称',
    model varchar(100) not null comment '仪器型号',
    manufacturer varchar(100) not null comment '生产厂商',
    status tinyint not null not comment '状态(1:启用，0:停用)',
    create_time bigint not null comment '创建时间',
    update_time bigint not null comment '更新时间',
    primary key(id),
    unique key uk_instrument_code(instrument_code)
) engine=innodb default charset=utf8mb4 comment '仪器基础信息(此表在LIS业务系统)';*/

create table instrument_plugin (
    id bigint not null auto_increment comment '主键ID',
    plugin_name varchar(100) not null comment '插件名称（如：迈瑞血球解析DLL）',
    dll_file_path varchar(500) not null comment 'DLL文件的路径（包含文件名）',
    ui_file_path varchar(500) not null comment 'UI文件的路径（包含文件名）',
    remark varchar(255) not null comment '备注',
    create_time bigint not null comment '创建时间',
    update_time bigint not null comment '更新时间',
    primary key (id)
) engine=innodb default charset=utf8mb4 comment '仪器插件表';

create table instrument_auth_code (
    id bigint not null auto_increment comment '主键ID',
    instrument_id bigint not null comment '仪器ID（关联仪器基础信息）',
    auth_code varchar(64) not null comment '仪器授权码（用于通信认证）',
    status tinyint not null comment '状态(1:正常 0:停用)',
    expire_time bigint comment '过期时间（时间戳）',
    --last_active_time bigint comment '最后通信时间',
    create_time bigint not null comment '创建时间',
    update_time bigint not null comment '更新时间',
    primary key (id),
    unique key uk_instrument_id(instrument_id),
    unique key uk_auth_code(auth_code)
) engine=innodb default charset=utf8mb4 comment '仪器授权码表';

create table instrument_item_mapping (
    id bigint not null auto_increment comment '主键ID',
    instrument_id bigint not null comment '仪器ID',
    instrument_item_code varchar(50) not null comment '仪器项目编码',
    instrument_item_name varchar(100) not null comment '仪器项目名称',
    lis_item_code varchar(50) not null comment 'LIS项目编码',
    enabled tinyint not null comment '是否启用(1:启用，0:停用)',
    create_time bigint not null comment '创建时间',
    update_time bigint not null comment '更新时间',
    primary key(id),
    key idx_instrument_id(instrument_id),
    key idx_lis_item_code(lis_item_code)
) engine=innodb default charset=utf8mb4 comment '仪器项目映射表';

create table instrument_config (
    id bigint not null auto_increment comment '主键ID',
    instrument_id bigint not null comment '仪器ID',
    protocol_type varchar(20) not null comment '通信协议：ASTM、HL7、自定义',
    connect_type varchar(20) not null comment '连接方式：Serial、TcpClient、TcpServer',
    port_name varchar(20) not null comment '串口名称',
    baud_rate int not null comment '波特率',
    data_bits int not null comment '数据位',
    parity varchar(10) not null comment '校验位',
    stop_bits int not null comment '停止位',
    ip varchar(100) not null comment 'TCP地址',
    port int not null comment 'TCP端口',
    enabled tinyint not null comment '是否启用(1:启用, 0:停用)',
    create_time bigint not null comment '创建时间',
    update_time bigint not null comment '更新时间',
    primary key(id),
    unique key uk_instrument_id(instrument_id)
) engine=innodb default charset=utf8mb4 comment '仪器通信配置表';

create table receive_message (
    id bigint not null auto_increment comment '主键ID',
    instrument_id bigint not null comment '仪器ID',
    status tinyint not null comment '状态(0:待处理, 1:处理成功, 2:处理失败)',
    --call_time bigint not null comment '任务执行触发时间',
    --retry_count tinyint not null comment '重试次数',
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
    barcode varchar(50) not null comment '条形码',
    result_json longtext not null comment '解码结果JSON',
    --status tinyint not null comment '状态：0待处理，1处理成功，2处理失败',
    create_time bigint not null comment '创建时间',
    update_time bigint not null comment '更新时间',
    primary key(id),
    unique key uk_receive_message_id(receive_message_id)
) engine=innodb default charset=utf8mb4 comment '接收仪器消息解码结果表';


create table send_message (
    id bigint not null auto_increment comment '主键ID',
    instrument_id bigint not null comment '仪器ID',
    type tinyint not null comment '类型(0:申请单)',
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
    --status tinyint not null comment '状态(0:待处理，1:处理成功，2:处理失败)',
    --ack_time bigint default null comment '仪器确认时间',
    --error_message varchar(500) not null comment '失败原因',
    create_time bigint not null comment '创建时间',
    update_time bigint not null comment '更新时间', 
    primary key(id),
    unique key uk_send_message_id(send_message_id)
) engine=innodb default charset=utf8mb4 comment '发送仪器消息编码记录';

/**
create table instrument_runtime (
    id bigint not null auto_increment comment '主键ID',
    instrument_id bigint not null comment '仪器ID',
    connect_status tinyint not null default 0 comment '连接状态：0离线，1在线',
    last_connect_time bigint default null comment '最后连接时间',
    last_receive_time bigint default null comment '最后接收时间',
    last_send_time bigint default null comment '最后发送时间',
    error_message varchar(500) default null comment '异常信息',
    create_time bigint not null comment '创建时间',
    update_time bigint not null comment '更新时间',
    primary key(id),
    key idx_instrument_id(instrument_id)
) engine=innodb default charset=utf8mb4 comment '仪器运行状态表';
**/

create table client_log (
    id bigint not null auto_increment comment '主键ID',
    instrument_id bigint default null comment '仪器ID',
    level tinyint not null comment '状态(0:普通，1:警告，2:异常)',
    message longtext default null comment '日志内容',
    create_time bigint not null comment '创建时间',
    primary key(id)
) engine=innodb default charset=utf8mb4 comment '仪器通信日志表';