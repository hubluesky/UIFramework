require "Class"

LuaModel = class()

function LuaModel:Ctor(modelName)
    local modelProxy = CS.VBM.ModelProxy()
    modelProxy.GetPropertyFunc = function(propertyName)
        return self[propertyName]
    end
    ModelManager.Instance:RegisterModel(modelName, modelProxy)

    local propertyMap = {}
    local metatable = getmetatable(self)
    metatable.__index = function(table, key)
        return propertyMap[key] or metatable[key]
    end

    metatable.__newindex = function(table, key, value)
        if propertyMap[key] ~= value then
            propertyMap[key] = value
            modelProxy:NotifyPropertyChanged(key, value)
        end
    end
end
