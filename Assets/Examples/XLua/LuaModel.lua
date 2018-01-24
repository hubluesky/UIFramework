require "Class"

LuaModel = class()

function LuaModel:Ctor()
    local modelProxy = CS.ModelProxy()
    modelProxy.GetPropertyFunc = function(propertyName)
        return self[propertyName]
    end

    modelProxy.GetMethodFunc = function(funcName)
        return self[funcName]
    end

    modelProxy.GetMethodParam1Func = function(funcName)
        return self[funcName]
    end

    ModelManager.Instance:RegisterModel(self:GetType(), modelProxy)

    local propertyMap = {}
    local metatable = getmetatable(self)
    metatable.propertyMap = propertyMap
    metatable.__index = function(table, key)
        return propertyMap[key] or metatable[key]
    end

    metatable.__newindex = function(table, key, value)
        if propertyMap[key] ~= value then
            propertyMap[key] = value
            modelProxy:NotifyPropertyChanged(key, value)
        end
    end

    if self.DeclareProperties ~= nil then
        self:DeclareProperties()
    end
end
