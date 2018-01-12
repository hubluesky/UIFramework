
--
-------------------------------------------------------------------------------------------------------------
function TableName(tbl)
  for k, v in pairs(_G) do
    if v == tbl then
	  return k
    end
  end
  return nil
end

--
-------------------------------------------------------------------------------------------------------------
function GetClass(name)
  return _G[name]
end

--
-------------------------------------------------------------------------------------------------------------
function InstanceOf(cls, type)
    for i, v in ipairs(cls.typeList) do
        if v == type then return true end
    end
    return false
end

--
-------------------------------------------------------------------------------------------------------------
local function SearchTypes(cls, typeList)
    table.insert(typeList, TableName(cls))
    if cls.base ~= nil then
        SearchTypes(cls.base, typeList)
    end
end

--
-------------------------------------------------------------------------------------------------------------
function class(super)
    local cls
    if super ~= nil then
        cls = {}
        cls.base = super
        setmetatable(cls, { __index = super })
    else
        cls = { Ctor = function() end }
    end

    cls.__index = cls
    function cls.new(...)
        local instance = setmetatable({}, cls)
        instance:Ctor(...)
        return instance
    end
    return cls
end

--
-------------------------------------------------------------------------------------------------------------
function registerType(cls)
    cls.typeList = {}
    cls.GetType = function() return cls.typeList[1] end
    SearchTypes(cls, cls.typeList)
end