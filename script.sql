drop database if exists bdManuEquip;
create database bdManuEquip;
use bdManuEquip;

create table Usuarios(
id_usuario int primary key auto_increment,
id_tecnico int,
nome varchar(100) not null,
email varchar(100) not null,
senha varchar(100) not null,
cep varchar(9),
role enum('Comum', 'Técnico', 'Gerente', 'Admin') not null default 'Comum',
criado_em datetime default now()
);

create table Equipamentos(
id_equipamento int primary key auto_increment,
id_usuario int,
nome varchar(100) not null,
modelo varchar(100) not null default 'Não identificado',
foto varchar(255),
situacao enum('Sem ordem', 'Em análise', 'Em manutenção', 'Em reparo', 'Em revisão', 'Comprometido', 'Em boas condições') not null default 'Sem ordem',
criado_em datetime default now()
);

create table Tecnicos(
id_tecnico int primary key auto_increment,
nome varchar(100) not null,
especialidade varchar(100) not null,
situacao enum('Disponível', 'Ocupado', 'Indisponível') not null default 'Disponível'
);

create table OrdensServico(
id_ordem int primary key auto_increment,
id_equipamento int not null,
id_tecnico int,
titulo varchar(50) not null,
mensagem varchar(255),
situacao enum('Em espera', 'Em andamento', 'Concluído') not null default 'Em espera',
criado_em datetime default now()
);

create table Relatorios(
id_relatorio int primary key auto_increment,
id_ordem int,
id_tecnico int,
assunto enum('Disponibilidade', 'Equipamento', 'Outro') not null,
criado_em datetime default now()
);

alter table OrdensServico add constraint fk_ordens_id_equip foreign key (id_equipamento) references Equipamentos(id_equipamento),
                          add constraint fk_ordens_id_tecnico foreign key (id_tecnico) references Tecnicos(id_tecnico);
                          
alter table Relatorios add constraint fk_relatorios_id_tecnico foreign key (id_tecnico) references Tecnicos(id_tecnico),
					   add constraint fk_relatorios_id_ordem foreign key (id_ordem) references OrdensServico(id_ordem);

alter table Usuarios add constraint fk_usuarios_id_tecnico foreign key (id_tecnico) references Tecnicos(id_tecnico);

alter table Equipamentos add constraint fk_equipamentos_id_usuario foreign key (id_usuario) references Usuarios(id_usuario);

/*delimiter $$
drop procedure if exists cadastrarUsuario; $$
create procedure cadastrarUsuario (in c_nome varchar(100), in c_email varchar(100), in c_senha varchar(100), in c_cep_inicio varchar(5), in c_cep_fim varchar(3))
begin

end; $$*/

delimiter $$
drop procedure if exists cadastrarUsuario; $$
create procedure cadastrarUsuario (in c_nome varchar(100), in c_espec varchar(100), in c_email varchar(100), in c_senha varchar(100), in c_cep_inicio varchar(5), in c_cep_fim varchar(3), c_role varchar(7))
begin
	if c_role != 'Técnico' then
		insert into Usuarios(nome, email, senha, cep, role)
					values(c_nome, c_email, c_senha, concat(c_cep_inicio, '-', c_cep_fim), c_role);
	else
		insert into Tecnicos(nome, especialidade, situacao)
					values(c_nome, c_espec, 'Disponivel');
		insert into Usuarios(id_tecnico, nome, email, senha, cep, role)
					values(last_insert_id(), c_nome, c_email, c_senha, concat(c_cep_inicio, '-', c_cep_fim), c_role);
	end if;
end; $$

call cadUsuario('Miguel', null, 'Miguel@gmail.com', '12234', '21212', '321', 'Comum');
call cadUsuario('Alexandre', 'Patinetes', 'Alexandre@gmail.com', '12343', '01234', '123', 'Técnico');

select * from Tecnicos;
select * from Usuarios;

delimiter $$
drop procedure if exists cadastrarEquipamento; $$
create procedure cadastrarEquip (in c_usuario int, in c_nome varchar(100), in c_modelo varchar(100))
begin
	insert into Equipamentos(id_usuario, nome, modelo)
				values(c_usuario, c_nome, c_modelo);
end; $$

delimiter $$
drop procedure if exists cadastrarOrdem; $$
create procedure cadastrarOrdem(in p_equip int, in p_tec int, in p_msg varchar(255))
begin
	insert into OrdensServico(id_equipamento, mensagem)
				values (p_equip, p_msg);
end; $$

delimiter $$
drop procedure if exists cadastrarRelatorio; $$
create procedure cadastrarRelatorio(in p_ordem int, in p_tecnico int, in p_assunto varchar(15))
begin
	insert into Relatorios(id_ordem, id_tecnico, assunto)
				values (p_ordem, p_tecnico, p_assunto);
end; $$


call cadEquip(1, 'Ventilador', 'Mundial');
call cadOrdem(1, 1, 'Ventilador ficando quente rápido não sei por que');

select * from Equipamentos;
select * from OrdensServico;
/*delimiter $$
drop procedure if exists listarUsuarios; $$
create procedure listarUsuarios()
begin
    if c_role is null then
        select distinct nome, email, criado_em from Usuarios order by nome;
    elseif p_role != 'Técnico' then
        select distinct nome, email from Usuarios 
        where role = p_role 
        order by nome;
    else
        select distinct u.nome, u.email, u.role, u.criado_em, t.especialidade as tecnico_especialidade, t.situacao as tecnico_situacao
        from Usuarios u
        left join Tecnicos t on t.id_tecnico = u.id_tecnico 
        where u.role = 'Técnico' 
        order by u.nome;
    end if;
end $$

delimiter $$
drop procedure if exists listarTecnicos; $$
create procedure listarTecnicos(in p_role varchar(7))
begin
	select distinct u.nome, u.email, u.role, u.criado_em, t.especialidade as tecnico_especialidade, t.situacao as tecnico_situacao
    from Usuarios u
    left join Tecnicos t on t.id_tecnico = u.id_tecnico 
    where u.role = 'Técnico' 
    order by u.nome;
end $$
*/

delimiter $$
drop procedure if exists listarUsuarios; $$
create procedure listarUsuarios(in p_role varchar(7))
begin
    if p_role is null then
        select distinct nome, email, criado_em from Usuarios order by nome;
    elseif p_role != 'Técnico' then
        select distinct nome, email from Usuarios 
        where role = p_role 
        order by nome;
    else
        select distinct u.nome, u.email, u.role, u.criado_em, t.especialidade as tecnico_especialidade, t.situacao as tecnico_situacao
        from Usuarios u
        left join Tecnicos t on t.id_tecnico = u.id_tecnico 
        where u.role = 'Técnico' 
        order by u.nome;
    end if;
end $$

delimiter $$
drop procedure if exists listarEquipamentos; $$
create procedure listarEquipamento()
begin
	select distinct nome, modelo, foto from Equipamentos order by nome;
end;

delimiter $$
drop procedure if exists listarOrdens; $$
create procedure listarOrdens()
begin
	select distinct o.titulo, t.nome as tecnico_nome, t.especialidade as tecnico_especialidade, o.situacao, o.criado_em from OrdensServico o
    left join Tecnicos t on t.id_tecnico = o.id_tecnico order by o.titulo;
end; $$

delimiter $$
drop procedure if exists listarRelatorios; $$
create procedure listarRelatorios()
begin
	select distinct o.titulo as ordem_titulo, t.nome, r.assunto as tecnico_nome, r.criado_em
	from Relatorios r
	left join OrdensServico o on o.id_ordem = r.id_ordem
	left join Tecnicos t on t.id_tecnico = r.id_tecnico;
end; $$